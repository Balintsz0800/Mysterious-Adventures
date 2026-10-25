using UnityEngine;

public class ClockUI : MonoBehaviour
{
    public Animator anim;
    public void Sunrise()
    {
        anim.SetTrigger("Sunrise");
    }
    public void MidDay()
    {
        anim.SetTrigger("Midday");
    }
    public void Sunset()
    {
        anim.SetTrigger("Sunset");
    }
    public void Night()
    {
        anim.SetTrigger("Night");
    }
}
