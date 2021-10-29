using UnityEngine;
using UnityEngine.UI;

public class NavigationIconInstance : MonoBehaviour
{
	public int IconIndex;
	
	public Image IconActive;
	public Image IconInactive;
	
	public Transform IconPosition;
	public Transform TopPosition;
	public Transform BelowPosition;
	
	Color normal = Color.white;
	
	public void SendCall(){
		BottomNavigation.RecieveCall(IconIndex);
	}
	
	public void SetState(bool to){
		if(to){
			LeanTween.value(gameObject, 0, 1, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnUpdate(
				(float val) =>
				{
					if(IconActive != null) IconActive.color = new Color(normal.r, normal.g, normal.b, val);
				}
			);
			LeanTween.value(gameObject, 1, 0, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnUpdate(
				(float val) =>
				{
					if(IconInactive != null) IconInactive.color = new Color(normal.r, normal.g, normal.b, val);
				}
			);
			IconPosition.LeanMove(TopPosition.position, 0.5f).setEase(LeanTweenType.easeOutCubic);
		}else{
			LeanTween.value(gameObject, 1, 0, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnUpdate(
				(float val) =>
				{
					if(IconActive != null) IconActive.color = new Color(normal.r, normal.g, normal.b, val);
				}
			);
			LeanTween.value(gameObject, 0, 1, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnUpdate(
				(float val) =>
				{
					if(IconInactive != null) IconInactive.color = new Color(normal.r, normal.g, normal.b, val);
				}
			);
			IconPosition.LeanMove(BelowPosition.position, 0.5f).setEase(LeanTweenType.easeOutCubic);
		}
	}
}
