using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShadowMonsters.Components;
using ShadowMonsters.ConversationComponents;
using ShadowMonsters.GameStates;
using ShadowMonsters.TileEngine;
using System;
using System.Collections.Generic;

namespace ShadowMonsters
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static Player Player;
        public static Random Random = new Random();
        private static Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private readonly GamePlayState gamePlayState;
        private readonly ConversationState conversationState;
        private readonly LevelUpState levelUpState;
        private readonly BattleOverState battleOverState;
        private readonly DamageState damageState;
        private readonly BattleState battleState;
        private readonly ShopState shopState;
        private readonly ActionSelectionState actionSelectionState;
        private readonly ShadowMonsterSelectionState shadowMonsterSelectionState;
        private readonly StartBattleState startBattleState;
        private readonly GameStateManager stateManager;
        private readonly ItemSelectionState itemSelectionState;
        private readonly UseItemState useItemState;
        private readonly MainMenuState mainMenuState;
        private readonly OptionState optionState;
        private readonly NewGameState newGameState;
        private readonly StartMonsterBattleState startMonsterBattleState;

        public StartMonsterBattleState StartMonsterBattleState => startMonsterBattleState;
        public NewGameState NewGameState => newGameState;

        private readonly TitleState titleState;
        private readonly LoadingState loadingState;
        private readonly YesNoState yesNoState;
        private readonly BindFailureState bindFailureState;
        private readonly BindSuccessState bindSuccessState;
        private readonly MessageState messageState;

        public BindFailureState BindFailureState => bindFailureState;
        public BindSuccessState BindSuccessState => bindSuccessState;
        public MessageState MessageState => messageState;

        public TitleState TitleState => titleState;
        public LoadingState LoadingState => loadingState;
        public YesNoState YesNoState => yesNoState;

        public static readonly Dictionary<string, Point> Resolutions = 
            new Dictionary<string, Point>();

        public GraphicsDeviceManager GraphicsDeviceManager => graphics;
        public SpriteBatch SpriteBatch => spriteBatch;
        public GamePlayState GamePlayState => gamePlayState;
        public ConversationState ConversationState => conversationState;
        public LevelUpState LevelUpState => levelUpState;
        public BattleOverState BattleOverState => battleOverState;
        public DamageState DamageState => damageState;
        public BattleState BattleState => battleState;
        public ActionSelectionState ActionSelectionState => actionSelectionState;
        public ShadowMonsterSelectionState ShadowMonsterSelectionState => shadowMonsterSelectionState;
        public StartBattleState StartBattleState => startBattleState;
        public ShopState ShopState => shopState;
        public UseItemState UseItemState => useItemState;
        public ItemSelectionState ItemSelectionState => itemSelectionState;
        public MainMenuState MainMenuState => mainMenuState;
        public OptionState OptionState => optionState;
        public GraphicsDeviceManager Graphics => graphics;
        public static Dictionary<AnimationKey, Animation> Animations => animations;

        public Game1()
        {
            Settings.Load();

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Settings.Resolution.X,
                PreferredBackBufferHeight = Settings.Resolution.Y
            };

            graphics.ApplyChanges();


            foreach (var v in graphics.GraphicsDevice.Adapter.SupportedDisplayModes)
            {
                Point p = new Point(v.Width, v.Height);
                string s = v.Width + " by " + v.Height + " pixels";

                if (v.Width >= 1280 && v.Height >= 720)
                {
                    Resolutions.Add(s, p);
                }
            }

            Content.RootDirectory = "Content";

            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            Components.Add(new Muse(this));
            Muse.SetEffectVolume(Settings.SoundVolume);
            Muse.SetSongVolume(Settings.MusicVolume);

            gamePlayState = new GamePlayState(this);
            conversationState = new ConversationState(this);
            levelUpState = new LevelUpState(this);
            damageState = new DamageState(this);
            battleOverState = new BattleOverState(this);
            battleState = new BattleState(this);
            actionSelectionState = new ActionSelectionState(this);
            shadowMonsterSelectionState = new ShadowMonsterSelectionState(this);
            startBattleState = new StartBattleState(this);
            shopState = new ShopState(this);
            itemSelectionState = new ItemSelectionState(this);
            useItemState = new UseItemState(this);
            mainMenuState = new MainMenuState(this);
            optionState = new OptionState(this);
            newGameState = new NewGameState(this);
            titleState = new TitleState(this);
            yesNoState = new YesNoState(this);
            loadingState = new LoadingState(this);
            startMonsterBattleState = new StartMonsterBattleState(this);
            bindFailureState = new BindFailureState(this);
            bindSuccessState = new BindSuccessState(this);
            messageState = new MessageState(this);

            stateManager.PushState(titleState);
            ConversationManager.Instance.CreateConversations(this);
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Settings.Load();

            graphics.PreferredBackBufferWidth = Settings.Resolution.X;
            graphics.PreferredBackBufferHeight = Settings.Resolution.Y;
            graphics.ApplyChanges();

            BuildAnimations();

            Components.Add(new Xin(this));
            Components.Add(new FontManager(this));

            base.Initialize();
        }

        public static void BuildAnimations()
        {
            Animation animation = new Animation(3, 32, 36, 0, 0);
            animations.Add(AnimationKey.WalkUp, animation);

            animation = new Animation(3, 32, 36, 0, 36);
            animations.Add(AnimationKey.WalkRight, animation);

            animation = new Animation(3, 32, 36, 0, 72);
            animations.Add(AnimationKey.WalkDown, animation);

            animation = new Animation(3, 32, 36, 0, 108);
            animations.Add(AnimationKey.WalkLeft, animation);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Muse.SoundEffects.Add(
                "menu_click",
                Content.Load<SoundEffect>(@"SoundEffects\Menu Selection Click"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}