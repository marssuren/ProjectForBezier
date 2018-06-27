using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMgr : MonoBehaviour
{
	public static PanelMgr Instance;
	private Dictionary<PanelName, PanelBase> loadedPanel;
	void Awake()
	{
		Instance = this;
	}

	public void LoadPanel(PanelName _panelName)
	{
		PanelBase tPanelBase = Resources.Load<PanelBase>("123");
		if(!loadedPanel.ContainsKey(_panelName))
		{
			loadedPanel.Add(_panelName, tPanelBase);
		}
	}
	public void Open(PanelName _panelName)
	{
		if(loadedPanel.ContainsKey(_panelName))
		{
			PanelBase tPanel = loadedPanel[_panelName];
			tPanel.Show();
		}
		else
		{
			LoadPanel(_panelName);
		}
	}
	public void Close(PanelName _panelName)
	{
		PanelBase tPanelBase = loadedPanel[_panelName];
		if(tPanelBase.gameObject.activeSelf)
		{
			tPanelBase.Hide();
		}
	}
	public void Destroy(PanelName _panelName)
	{
		if (loadedPanel.ContainsKey(_panelName))
		{
			PanelBase tPanelBase = loadedPanel[_panelName];
			tPanelBase.Hide();
			Destroy(tPanelBase);
		}
	}

}
public enum PanelName
{
	None = 0,
	MainPanel = 1,
}
