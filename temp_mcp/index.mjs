import { Client } from "@modelcontextprotocol/sdk/client/index.js";
import { SSEClientTransport } from "@modelcontextprotocol/sdk/client/sse.js";
import fs from "fs";

const API_KEY = process.env.PIXELLAB_API_KEY || "704a1bcc-104f-4bee-a729-20de458a039d";
const token = API_KEY.replace("Bearer ", "").trim();

async function main() {
    console.log("Initializing SSE Client Transport...");
    // Using URL object
    const transport = new SSEClientTransport(new URL("https://api.pixellab.ai/mcp"), {
        headers: {
            Authorization: `Bearer ${token}`
        }
    });

    const client = new Client({
        name: "demontek-client",
        version: "1.0.0"
    });

    console.log("Connecting...");
    await client.connect(transport);
    console.log("Connected to MCP Server!");

    try {
        console.log("Starting Dungeon generation...");
        const dungeonResult = await client.callTool({
            name: "create_sidescroller_tileset",
            arguments: {
                lower_description: "dark stone blocks, mossy, dungeon",
                transition_description: "stone floor",
                tile_size: { width: 16, height: 16 }
            }
        });
        console.log("Dungeon Result:", JSON.stringify(dungeonResult, null, 2));

        console.log("Starting Castle generation...");
        const castleResult = await client.callTool({
            name: "create_sidescroller_tileset",
            arguments: {
                lower_description: "castle brick wall, well-lit",
                transition_description: "red carpet, elegant",
                tile_size: { width: 16, height: 16 }
            }
        });
        console.log("Castle Result:", JSON.stringify(castleResult, null, 2));

        const out = {
            dungeon: dungeonResult,
            castle: castleResult
        };
        fs.writeFileSync("tileset_jobs.json", JSON.stringify(out, null, 2));

    } catch (e) {
        console.error("Tool execution failed:", e);
    }

    process.exit(0);
}

main().catch(console.error);
