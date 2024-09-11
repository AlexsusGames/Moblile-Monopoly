using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CasinoView : MonoBehaviour
{
    [SerializeField] private List<Image> slots = new();
    [SerializeField] private List<Image> winableSlots = new();
    [SerializeField] private List<Sprite> sprites = new();
    [SerializeField] private int animationDuration;
    private Animator animator;

    public event UnityAction OnAnimPlayed;

    private void Awake() => animator = GetComponent<Animator>();

    public void SetData(int[] combination)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].sprite = sprites[Random.Range(0, sprites.Count)];
        }
        for (int i = 0; i < combination.Length; i++)
        {
            winableSlots[i].sprite = sprites[combination[i]];
        }
    }
    public async void PlayAnim()
    {
        animator.enabled = true;
        await Task.Delay(animationDuration);
        animator.enabled = false;
        OnAnimPlayed?.Invoke();
        OnAnimPlayed = null;
    }
}
