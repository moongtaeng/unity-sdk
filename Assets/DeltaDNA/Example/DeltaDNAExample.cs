﻿using UnityEngine;
using System.Collections.Generic;
using DeltaDNA;
using DeltaDNA.Messaging;

public class DeltaDNAExample : MonoBehaviour {

    public const string ENVIRONMENT_KEY = "76410301326725846610230818914037";
    public const string COLLECT_URL = "http://collect2470ntysd.deltadna.net/collect/api";
    public const string ENGAGE_URL = "http://engage2470ntysd.deltadna.net";
    public const string ENGAGE_TEST_URL = "http://www.deltadna.net/qa/engage";

    private string popupContent = "";
    private string popupTitle = "DeltaDNA Example";
    
    // Use this for initialization
    void Start () {
    
        // Configure the SDK
        DDNA.Instance.SetLoggingLevel(DeltaDNA.Logger.Level.DEBUG);
        DDNA.Instance.HashSecret = "1VLjWqChV2YC1sJ4EPKGzSF3TbhS26hq";
        DDNA.Instance.ClientVersion = "1.0.0";

        // Enable push notifications
        DDNA.Instance.IosNotifications.OnDidRegisterForPushNotifications += (string n) => { Debug.Log ("Got an iOS push token: "+n);};
        DDNA.Instance.IosNotifications.OnDidReceivePushNotification += (string n) => { Debug.Log ("Got an iOS push notification! "+n);};
        DDNA.Instance.IosNotifications.RegisterForPushNotifications();

        DDNA.Instance.AndroidNotifications.OnDidRegisterForPushNotifications += (string n) => {
            Debug.Log("Got an Android registration token: " + n);
        };
        DDNA.Instance.AndroidNotifications.OnDidFailToRegisterForPushNotifications += (string n) => {
            Debug.Log("Failed getting an Android registration token: " + n);
        };
        DDNA.Instance.AndroidNotifications.RegisterForPushNotifications();

        // Start collecting data
        DDNA.Instance.StartSDK(ENVIRONMENT_KEY, COLLECT_URL, ENGAGE_URL);
    }

    // Update is called once per frame
    void Update () {

    }

    void FixedUpdate() {
        // Make our cube rotate
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }

    void OnGUI() {

        int x = 10;
        int y = 10;
        int width = 180;
        int height = 70;
        int space = height + 5;

        GUI.skin.textField.wordWrap = true;
        GUI.skin.button.fontSize = 18;

        if (GUI.Button(new Rect(x, y, width, height), "Simple Event")) {

            GameEvent gameEvent = new GameEvent("options")
                .AddParam("option", "sword")
                .AddParam("action", "sell");

            DDNA.Instance.RecordEvent(gameEvent);

        }

        if (GUI.Button(new Rect(x, y += space, width, height), "Achievement Event")) {

            GameEvent gameEvent = new GameEvent("achievement")
                .AddParam("achievementName", "Sunday Showdown Tournament Win")
                .AddParam("achievementID", "SS-2014-03-02-01")
                .AddParam("reward", new Params()
                    .AddParam("rewardName", "Medal")
                    .AddParam("rewardProducts", new Product()
                        .SetRealCurrency("USD", 5000)
                        .AddVirtualCurrency("VIP Points", "GRIND", 20)
                        .AddItem("Sunday Showdown Medal", "Victory Badge", 1)
                    )
                );

            DDNA.Instance.RecordEvent(gameEvent);
                                    
        }

        if (GUI.Button(new Rect(x, y += space, width, height), "Transaction Event")) {

            Transaction transaction = new Transaction(
                "Weapon type 11 manual repair", 
                "PURCHASE", 
                new Product().AddItem("WeaponsMaxConditionRepair:11", "WeaponMaxConditionRepair", 5),
                new Product().AddVirtualCurrency("Credit", "GRIND", 710))
                .SetTransactorId("2.212.91.84:15116")
                .SetProductId("4019")
                .AddParam("paymentCountry", "GB");

            DDNA.Instance.RecordEvent(transaction);
        }

        if (GUI.Button(new Rect(x, y += space, width, height), "Engagement")) {

            var engagement = new Engagement("gameLoaded")
                .AddParam("userLevel", 4)
                .AddParam("experience", 1000)
                .AddParam("missionName", "Disco Volante");

            DDNA.Instance.RequestEngagement(engagement, (response) =>
            {
                popupContent = DeltaDNA.MiniJSON.Json.Serialize(response);
            });

            popupTitle = "Engage returned";
        }

        if (GUI.Button(new Rect(x, y += space, width, height), "Popup Image")) {

            // Create Popup Object
            IPopup imagePopup = new Popup();
            // Setup Events
            imagePopup.AfterPrepare += (sender, e) => {
                Debug.Log("Popup loaded resource");
                // Just show it, although you could do this later
                imagePopup.Show();
            };

            imagePopup.Dismiss += (sender, e) => {
                Debug.Log("Popup dismissed by "+e.ID);
            };

            imagePopup.Action += (sender, e) => {
                Debug.Log("Popup actioned by "+e.ID+" with command "+e.ActionValue);
            };

            // Start Request
            var engagement = new Engagement("pickUp")
                .AddParam("userScore", 42)
                .AddParam("secondsPlayed", 20);

            DDNA.Instance.RequestImageMessage(engagement, imagePopup);
        }

        if (GUI.Button(new Rect(x, y += space, width, height), "Notification Opened")) {
            var payload = new Dictionary<string, object>();
            payload.Add("_ddId", 1);
            payload.Add("_ddName", "Example Notification");
            payload.Add("_ddLaunch", true);
            DDNA.Instance.RecordPushNotification(payload);
        }

        if (GUI.Button(new Rect(x, y += space, width, height), "Upload Events")) {
            DDNA.Instance.Upload();
        }

        if (GUI.Button(new Rect(x, y += space, width, height), "Start SDK")) {
            DDNA.Instance.StartSDK(ENVIRONMENT_KEY, COLLECT_URL, ENGAGE_URL);
        }

        if (GUI.Button(new Rect(x, y += space, width, height), "Stop SDK")) {
            DDNA.Instance.StopSDK();
        }

        if (GUI.Button(new Rect(x, y += space, width, height), "New Session")) {
            DDNA.Instance.NewSession();
        }

        if (popupContent != "") {
            GUI.ModalWindow(0, new Rect(Screen.width/2-150, Screen.height/2-100, 300, 200), RenderPopupContent, popupTitle);
        }
    }

    void RenderPopupContent(int windowID) {
        if (GUI.Button(new Rect(248, 3, 50, 20), "Close")) {
            popupContent = "";
        }
        GUI.TextField(new Rect(0, 25, 300, 175), popupContent);
    }
}
