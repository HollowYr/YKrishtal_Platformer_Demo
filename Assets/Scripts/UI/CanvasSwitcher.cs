/***
 * Class for switching between UIGroups inside SafeArea.
 * Finds active group or group by name. 
 * Also disable active group and activates group by name.
 ***/

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Should be on Safe Area component and then call method from button or etc to change UI group
/// </summary>

public class CanvasSwitcher : MonoBehaviour
{
    List<UIGroup> UIGroupList;
    private void Start()
    {
        UIGroupList = new List<UIGroup>();
        foreach (Transform child in transform)
        {
            UIGroupList.Add(new UIGroup(child.gameObject));
        }
    }

    public void DisableActiveAndActivateUIByName(string UIGroupName)
    {
        UIGroup activeGroup = FindActiveUIGroup();
        if (activeGroup.GetName().Equals(UIGroupName))
        {
            return;
        }
        activeGroup.SetInactive();
        FindUIGroup(UIGroupName).SetActive();
    }
    /// <returns>Return First active UI group</returns>
    public UIGroup FindActiveUIGroup()
    {
        foreach (UIGroup ui in UIGroupList)
        {
            if (!ui.isActive()) continue;
            return ui;
        }
        Debug.LogWarning("There is no active UIGroup in the UIGroup List");
        return null;
    }

    /// <param name="UIGroupName">Name of the UI group that will be searched.</param> 
    /// <returns>Return first UI group that has same name.</returns>
    public UIGroup FindUIGroup(string UIGroupName)
    {
        foreach (UIGroup ui in UIGroupList)
        {
            if (ui.GetName() != UIGroupName) continue;
            return ui;
        }
        Debug.LogWarning("There is no " + UIGroupName + "in the UIGroup List");
        return null;
    }

    public class UIGroup
    {
        private GameObject UIScene;

        public UIGroup(GameObject UIScene)
        {
            this.UIScene = UIScene;
        }

        public bool isActive()
        {
            return UIScene.activeInHierarchy;
        }

        public string GetName()
        {
            return UIScene.name;
        }

        public void SetActive()
        {
            UIScene.SetActive(true);
        }

        public void SetInactive()
        {
            UIScene.SetActive(false);
        }
    }
}
