using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomNavigation : MonoBehaviour
{
	public Slider navigationSlider;
	public NavigationIconInstance[] navigationIcons;
	public int selectedIndex;
	public UnityEngine.Events.UnityEvent OnSelectionChanged;
	
	static BottomNavigation m_self;
	static BottomNavigation self {
		get
		{
			if(m_self == null) m_self = FindObjectOfType<BottomNavigation>();
			return m_self;
		}
		set
		{
			m_self = value;
		}
	}
	public static int GetCurrentIndex(){
		var navigation = self;
		if(navigation != null){
			return navigation.selectedIndex;
		}
		return 0;
	}
	public static void RecieveCall(int index){
		var navigation = self;
		if(navigation != null){
			navigation.selectedIndex = index;
			for(int i = 0; i < navigation.navigationIcons.Length; i++){
				navigation.navigationIcons[i].SetState(i == index);
			}
			LeanTween.value(navigation.gameObject, navigation.navigationSlider.value, index, 0.5f).setEaseOutCubic().setOnUpdate(
				(float val) =>
				{
					if(navigation != null) navigation.navigationSlider.value = val;
				}
			);
			navigation.OnSelectionChanged.Invoke();
		}
	}
}
