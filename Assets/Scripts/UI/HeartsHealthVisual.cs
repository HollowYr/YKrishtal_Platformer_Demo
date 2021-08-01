/***
 * Visual part of HeartHealthSystem. 
 * Updating sprites depending on health Amount.
 * Activating Death_UI group if the player isDead.
 ***/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeartsHealthVisual : MonoBehaviour
{
    internal static HeartHealthSystem heartHealthSystemStatic;
    [SerializeField] private Sprite heartSprite0;
    [SerializeField] private Sprite heartSprite1;
    [SerializeField] private Sprite heartSprite2;
    [SerializeField] private Sprite heartSprite3;
    [SerializeField] private Sprite heartSprite4;
    [SerializeField] private AnimationClip heartHealAnimationClip;
    [SerializeField] private Vector2 heartSize = new Vector2(50, 50);
    [SerializeField] private Vector2 heartOffset = new Vector2(80, 0);
    [SerializeField] private int heartAmount = 5;
    [SerializeField] private int heartColMax = 10;
    [SerializeField] private float heartRowColSize = 20f;
    [SerializeField] [Range(0.0f, 1f)] private float healAnimationDelay = 0.5f;


    [SerializeField] private CanvasSwitcher canvasSwitcher;
    [SerializeField] internal PlayerScript playerScript;


    private List<HeartImage> heartImageList;
    private HeartHealthSystem heartHealthSystem;

    private void Awake()
    {
        heartImageList = new List<HeartImage>();
    }

    public void Test_Damage(int damageAmount)
    {
        heartHealthSystem.Damage(damageAmount);
    }

    public void Test_Heal(int healAmount)
    {
        heartHealthSystem.Heal(healAmount);
    }

    private void Start()
    {
        SetHealthSystem();
    }

    private void SetHealthSystem()
    {
        HeartHealthSystem heartHealthSystem = new HeartHealthSystem(heartAmount, this);
        SetHeartHealthSystem(heartHealthSystem);
    }

    public void Resurrect()
    {
        canvasSwitcher.DisableActiveAndActivateUIByName("Game_UI");
        heartHealthSystem.Heal(heartAmount * HeartHealthSystem.MAX_FRAGMENT_AMOUNT);
        playerScript.ChangeAnimationState("Player_Idle");
    }

    public bool IsDead()
    {
        return heartHealthSystem.IsDead();
    }

    private void SetHeartHealthSystem(HeartHealthSystem heartHealthSystem)
    {
        this.heartHealthSystem = heartHealthSystem;
        heartHealthSystemStatic = heartHealthSystem;
        List<HeartHealthSystem.Heart> heartList = heartHealthSystem.GetHeartList();

        int row = 0, col = 0;
        for (int i = 0; i < heartList.Count; i++, col++)
        {
            if (col >= heartColMax)
            {
                row++;
                col = 0;
            }
            HeartHealthSystem.Heart heart = heartList[i];
            Vector2 heartAnchoredPosition = new Vector2(col * heartRowColSize + (col * heartOffset.x), -row * heartRowColSize + (-row * heartOffset.y));

            CreateHeartImage(heartAnchoredPosition).SetHeartFragment(heart.GetFragmentAmount());
        }

        heartHealthSystem.OnDamaged += HeartHealthSystem_OnDamaged;
        heartHealthSystem.OnHealed += HeartHealthSystem_OnHealed;
        heartHealthSystem.OnDead += HeartHealthSystem_OnDead;
    }

    private void HeartHealthSystem_OnDead(object sender, System.EventArgs e)
    {
        playerScript.ChangeAnimationState("Player_Die");
        canvasSwitcher.DisableActiveAndActivateUIByName("Death_UI");
    }

    private void HeartHealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        HealingAnimatedPeriodic();
    }

    private void HeartHealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        RefreshAllHearts();
    }

    private void RefreshAllHearts()
    {
        List<HeartHealthSystem.Heart> heartList = heartHealthSystem.GetHeartList();
        for (int i = 0; i < heartList.Count; i++)
        {
            HeartImage heartImage = heartImageList[i];
            HeartHealthSystem.Heart heart = heartList[i];

            heartImage.SetHeartFragment(heart.GetFragmentAmount());
        }
    }

    private void HealingAnimatedPeriodic()
    {
        StartCoroutine(RefreshAllHeartsFragmentaly());
    }

    IEnumerator RefreshAllHeartsFragmentaly()
    {
        bool isHealed = false;
        while (!isHealed)
        {
            List<HeartHealthSystem.Heart> heartList = heartHealthSystem.GetHeartList();
            for (int i = 0; i < heartList.Count; i++)
            {
                HeartImage heartImage = heartImageList[i];
                HeartHealthSystem.Heart heart = heartList[i];

                if (heartImage.GetFragmentAmount() != heart.GetFragmentAmount())
                {
                    //visuals is different from logic
                    heartImage.AddHeartVisualFragment();

                    if (heartImage.GetFragmentAmount() == HeartHealthSystem.MAX_FRAGMENT_AMOUNT)
                    {
                        heartImage.PlayHeartHealAnimation();
                    }

                    yield return new WaitForSeconds(healAnimationDelay);
                    break;
                }

                else if (i == heartList.Count - 1)
                {
                    isHealed = true;
                }
            }

        }
    }

    private HeartImage CreateHeartImage(Vector2 anchoredPositiom)
    {
        // Create GameObject
        GameObject heartGameObject = new GameObject("Heart", typeof(Image), typeof(Animation));

        // Set as child of this transform
        heartGameObject.transform.SetParent(transform);
        heartGameObject.transform.localPosition = Vector3.zero;

        // Locate and size heart
        heartGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPositiom;
        heartGameObject.GetComponent<RectTransform>().sizeDelta = heartSize;
        heartGameObject.GetComponent<RectTransform>().localScale = Vector3.one;


        heartGameObject.GetComponent<Animation>().AddClip(heartHealAnimationClip, "Heart_Heal");

        // Set heart sprite
        Image heartImageUI = heartGameObject.GetComponent<Image>();
        heartImageUI.sprite = heartSprite4;


        HeartImage heartImage = new HeartImage(this, heartImageUI, heartGameObject.GetComponent<Animation>());
        heartImageList.Add(heartImage);
        return heartImage;
    }

    // Represent a single Heart
    public class HeartImage
    {
        private int fragments;
        private Image heartImage;
        private HeartsHealthVisual heartsHealthVisual;
        private Animation healAnimation;
        public HeartImage(HeartsHealthVisual heartsHealthVisual, Image heartImage, Animation healAnimation)
        {
            this.heartsHealthVisual = heartsHealthVisual;
            this.heartImage = heartImage;
            this.healAnimation = healAnimation;
        }

        public void SetHeartFragment(int fragments)
        {
            this.fragments = fragments;
            switch (fragments)
            {
                case 0: heartImage.sprite = heartsHealthVisual.heartSprite0; break;
                case 1: heartImage.sprite = heartsHealthVisual.heartSprite1; break;
                case 2: heartImage.sprite = heartsHealthVisual.heartSprite2; break;
                case 3: heartImage.sprite = heartsHealthVisual.heartSprite3; break;
                case 4: heartImage.sprite = heartsHealthVisual.heartSprite4; break;
            }
        }

        public int GetFragmentAmount()
        {
            return fragments;
        }

        public void AddHeartVisualFragment()
        {
            SetHeartFragment(fragments + 1);
        }

        public void PlayHeartHealAnimation()
        {
            healAnimation.Play("Heart_Heal", PlayMode.StopAll);
        }
    }
}
