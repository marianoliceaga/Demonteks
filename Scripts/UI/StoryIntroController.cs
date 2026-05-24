using Godot;
using System;

namespace Demonteks.UI
{
    /// <summary>
    /// Cinematic story introduction controller.
    /// Displays illustrated slides with typewriter-effect narration text
    /// before transitioning to Level 1.
    /// </summary>
    public partial class StoryIntroController : Control
    {
        #region Story Data
        private struct StorySlide
        {
            public string ImagePath;
            public string NarrationText;
        }

        private static readonly StorySlide[] Level1Slides = new StorySlide[]
        {
            new StorySlide { ImagePath = "res://Assets/story/Level1/story_scene_1.png", NarrationText = "The explorer wakes up confused...\nHe stands up and looks at the sky\n— it is violet." },
            new StorySlide { ImagePath = "res://Assets/story/Level1/story_scene_2.png", NarrationText = "The plants are giant compared to those on Earth.\nEverything suggests a jungle\non another planet." },
            new StorySlide { ImagePath = "res://Assets/story/Level1/story_scene_3.png", NarrationText = "He encounters a new alien species.\nAt first, they seem friendly and adorable...\nHe starts joking about how small\nand silly they look." },
            new StorySlide { ImagePath = "res://Assets/story/Level1/story_scene_4.png", NarrationText = "They become enraged and begin to chase him!\nThese small demons are called Demonteks\n— because they repeatedly say \"tek\"\nand their forms resemble demons." }
        };

        private static readonly StorySlide[] Level2Slides = new StorySlide[]
        {
            new StorySlide { ImagePath = "res://Assets/story/Level2/story_scene_1.png", NarrationText = "Having survived the dense jungle,\nthe explorer finds an ancient dungeon entrance." },
            new StorySlide { ImagePath = "res://Assets/story/Level2/story_scene_2.png", NarrationText = "The damp walls echo with strange noises.\nThe Demonteks have followed him inside." },
            new StorySlide { ImagePath = "res://Assets/story/Level2/story_scene_3.png", NarrationText = "Traps and darker creatures await in the shadows,\nguarding forgotten secrets." },
            new StorySlide { ImagePath = "res://Assets/story/Level2/story_scene_4.png", NarrationText = "There's no turning back now.\nHe must push forward into the depths..." }
        };

        private static readonly StorySlide[] Level3Slides = new StorySlide[]
        {
            new StorySlide { ImagePath = "res://Assets/story/Level3/story_scene_1.png", NarrationText = "Escaping the dungeon, a massive castle looms over the horizon.\nThe stronghold of the Demonteks." },
            new StorySlide { ImagePath = "res://Assets/story/Level3/story_scene_2.png", NarrationText = "The Elite Demonteks patrol the heavy iron gates.\nThis will be his greatest challenge." },
            new StorySlide { ImagePath = "res://Assets/story/Level3/story_scene_3.png", NarrationText = "Somewhere inside lies the King Demontek,\nholding the key to his escape..." }
        };
        
        private StorySlide[] _activeSlides;
        private int _targetLevel = 1;
        #endregion

        #region Configuration
        private const float TypewriterSpeed = 0.04f;     // seconds per character
        private const float AutoAdvanceDelay = 2.5f;      // seconds after text finishes
        private const float FadeDuration = 0.8f;          // fade in/out duration
        private const string NextScenePath = "res://Scenes/Levels/Level1_Jungle.tscn";
        #endregion

        #region Node References
        private TextureRect _backgroundImage;
        private ColorRect _imageOverlay;
        private ColorRect _textPanel;
        private Label _narrationLabel;
        private Label _skipHint;
        private ColorRect _fadeRect;
        #endregion

        #region State
        private int _currentSlideIndex = 0;
        private int _visibleCharacters = 0;
        private bool _typingComplete = false;
        private bool _transitioning = false;
        private float _typewriterTimer = 0f;
        private float _autoAdvanceTimer = 0f;
        private string _currentFullText = "";
        private enum IntroState { FadingIn, Typing, WaitingAutoAdvance, FadingOut, TransitionOut }
        private IntroState _state = IntroState.FadingIn;
        private float _fadeTimer = 0f;
        #endregion

        public override void _Ready()
        {
            // Get node references
            _backgroundImage = GetNode<TextureRect>("BackgroundImage");
            _imageOverlay = GetNode<ColorRect>("ImageOverlay");
            _textPanel = GetNode<ColorRect>("TextPanel");
            _narrationLabel = GetNode<Label>("TextPanel/NarrationLabel");
            _skipHint = GetNode<Label>("SkipHint");
            _fadeRect = GetNode<ColorRect>("FadeRect");

            // Initialize
            _fadeRect.Visible = true;
            _fadeRect.Color = new Color(0, 0, 0, 1); // Start fully black
            _narrationLabel.Text = "";
            _skipHint.Text = "Press ESC to skip";

            // Determine Target Level and Slides
            var gameManager = GetNodeOrNull<Management.GameManager>("/root/GameManager");
            if (gameManager != null)
            {
                _targetLevel = gameManager.CurrentLevel;
            }

            if (_targetLevel == 3)
                _activeSlides = Level3Slides;
            else if (_targetLevel == 2)
                _activeSlides = Level2Slides;
            else
                _activeSlides = Level1Slides;

            // Load first slide
            LoadSlide(0);

            // Start or continue main menu music
            var audioManager = GetNodeOrNull<Management.AudioManager>("/root/AudioManager");
            audioManager?.PlayMusic("res://Assets/audio/music/MainTheme.mp3");

            GD.Print($"[StoryIntro] Story intro started for Level {_targetLevel}");
        }

        public override void _Process(double delta)
        {
            float dt = (float)delta;

            switch (_state)
            {
                case IntroState.FadingIn:
                    ProcessFadeIn(dt);
                    break;
                case IntroState.Typing:
                    ProcessTyping(dt);
                    break;
                case IntroState.WaitingAutoAdvance:
                    ProcessAutoAdvance(dt);
                    break;
                case IntroState.FadingOut:
                    ProcessFadeOut(dt);
                    break;
                case IntroState.TransitionOut:
                    ProcessTransitionOut(dt);
                    break;
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (_transitioning) return;

            // ESC to skip the entire intro
            if (@event is InputEventKey keyEvent && keyEvent.Pressed && !keyEvent.Echo)
            {
                if (keyEvent.Keycode == Key.Escape || keyEvent.PhysicalKeycode == Key.Escape)
                {
                    SkipIntro();
                    GetViewport().SetInputAsHandled();
                    return;
                }
            }

            // Any click/tap or key press to advance
            bool shouldAdvance = false;

            if (@event is InputEventKey key && key.Pressed && !key.Echo)
                shouldAdvance = true;
            else if (@event is InputEventMouseButton mouse && mouse.Pressed)
                shouldAdvance = true;
            else if (@event is InputEventScreenTouch touch && touch.Pressed)
                shouldAdvance = true;

            if (shouldAdvance && (_state == IntroState.Typing || _state == IntroState.WaitingAutoAdvance))
            {
                if (_state == IntroState.Typing)
                {
                    // Complete the current text immediately
                    _visibleCharacters = _currentFullText.Length;
                    _narrationLabel.Text = _currentFullText;
                    _typingComplete = true;
                    _state = IntroState.WaitingAutoAdvance;
                    _autoAdvanceTimer = 0f;
                }
                else if (_state == IntroState.WaitingAutoAdvance)
                {
                    // Advance to next slide
                    AdvanceSlide();
                }
                GetViewport().SetInputAsHandled();
            }
        }

        #region Slide Management
        private void LoadSlide(int index)
        {
            if (index < 0 || index >= _activeSlides.Length) return;

            _currentSlideIndex = index;
            var slide = _activeSlides[index];

            // Load background image
            if (ResourceLoader.Exists(slide.ImagePath))
            {
                var texture = GD.Load<Texture2D>(slide.ImagePath);
                _backgroundImage.Texture = texture;
            }
            else
            {
                GD.PushWarning($"[StoryIntro] Image not found: {slide.ImagePath}");
            }

            // Reset text state
            _currentFullText = slide.NarrationText;
            _visibleCharacters = 0;
            _narrationLabel.Text = "";
            _typingComplete = false;
            _typewriterTimer = 0f;
            _autoAdvanceTimer = 0f;

            // Start fade in
            _state = IntroState.FadingIn;
            _fadeTimer = 0f;
            _fadeRect.Color = new Color(0, 0, 0, 1);

            GD.Print($"[StoryIntro] Loading slide {index + 1}/{_activeSlides.Length}");
        }

        private void AdvanceSlide()
        {
            if (_currentSlideIndex < _activeSlides.Length - 1)
            {
                // Fade out, then load next slide
                _state = IntroState.FadingOut;
                _fadeTimer = 0f;
            }
            else
            {
                // Last slide — transition to game
                TransitionToLevel();
            }
        }
        #endregion

        #region State Processing
        private void ProcessFadeIn(float dt)
        {
            _fadeTimer += dt;
            float progress = Mathf.Clamp(_fadeTimer / FadeDuration, 0f, 1f);

            // Ease out
            float alpha = 1f - EaseOutCubic(progress);
            _fadeRect.Color = new Color(0, 0, 0, alpha);

            if (progress >= 1f)
            {
                _fadeRect.Color = new Color(0, 0, 0, 0);
                _state = IntroState.Typing;
                _typewriterTimer = 0f;
            }
        }

        private void ProcessTyping(float dt)
        {
            _typewriterTimer += dt;

            if (_typewriterTimer >= TypewriterSpeed)
            {
                _typewriterTimer -= TypewriterSpeed;
                _visibleCharacters++;

                if (_visibleCharacters >= _currentFullText.Length)
                {
                    _visibleCharacters = _currentFullText.Length;
                    _narrationLabel.Text = _currentFullText;
                    _typingComplete = true;
                    _state = IntroState.WaitingAutoAdvance;
                    _autoAdvanceTimer = 0f;
                }
                else
                {
                    _narrationLabel.Text = _currentFullText.Substring(0, _visibleCharacters);
                }
            }
        }

        private void ProcessAutoAdvance(float dt)
        {
            _autoAdvanceTimer += dt;
            if (_autoAdvanceTimer >= AutoAdvanceDelay)
            {
                AdvanceSlide();
            }
        }

        private void ProcessFadeOut(float dt)
        {
            _fadeTimer += dt;
            float progress = Mathf.Clamp(_fadeTimer / FadeDuration, 0f, 1f);

            float alpha = EaseInCubic(progress);
            _fadeRect.Color = new Color(0, 0, 0, alpha);

            if (progress >= 1f)
            {
                _fadeRect.Color = new Color(0, 0, 0, 1);
                // Load the next slide
                LoadSlide(_currentSlideIndex + 1);
            }
        }

        private void ProcessTransitionOut(float dt)
        {
            _fadeTimer += dt;
            float progress = Mathf.Clamp(_fadeTimer / FadeDuration, 0f, 1f);

            float alpha = EaseInCubic(progress);
            _fadeRect.Color = new Color(0, 0, 0, alpha);

            if (progress >= 1f && !_transitioning)
            {
                _transitioning = true;
                LoadNextScene();
            }
        }
        #endregion

        #region Transitions
        private void TransitionToLevel()
        {
            _state = IntroState.TransitionOut;
            _fadeTimer = 0f;
        }

        private void SkipIntro()
        {
            if (_transitioning) return;
            GD.Print("[StoryIntro] Intro skipped by player");
            TransitionToLevel();
        }

        private void LoadNextScene()
        {
            GD.Print($"[StoryIntro] Transitioning to Level {_targetLevel}...");

            var gameManager = GetNodeOrNull<Management.GameManager>("/root/GameManager");
            if (gameManager != null)
            {
                gameManager.LoadLevel(_targetLevel);
            }
            else
            {
                GetTree().ChangeSceneToFile(NextScenePath);
            }
        }
        #endregion

        #region Utility
        private static float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);
        private static float EaseInCubic(float t) => t * t * t;
        #endregion
    }
}
