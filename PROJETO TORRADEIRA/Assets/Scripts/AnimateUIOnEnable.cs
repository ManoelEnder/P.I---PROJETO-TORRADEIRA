using UnityEngine;
using DG.Tweening;

public class AnimateUIOnEnable : MonoBehaviour
{
    [Header("Animation Settings")]
    public float duration = 0.5f;
    public float delayBetween = 0.1f;
    public Vector3 startScale = new Vector3(0f, 0f, 0f);
    public Ease easeType = Ease.OutBack;

    private void OnEnable()
    {
        AnimateChildren();
    }

    private void AnimateChildren()
    {
        int index = 0;

        foreach (RectTransform child in GetComponentsInChildren<RectTransform>(true))
        {
            if (child == this.transform) continue; // ignora o próprio objeto root

            // Cancela animações antigas
            child.DOKill(true);

            CanvasGroup cg = child.GetComponent<CanvasGroup>();
            if (cg == null)
                cg = child.gameObject.AddComponent<CanvasGroup>();

            // Garante que sempre vai ser interativo
            cg.interactable = true;
            cg.blocksRaycasts = true;

            // Estado inicial (antes da animação)
            child.localScale = startScale;
            cg.alpha = 0f;

            // Sequência da animação
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(index * delayBetween);
            seq.Append(child.DOScale(Vector3.one, duration).SetEase(easeType));
            seq.Join(cg.DOFade(1f, duration));

            index++;
        }
    }
}
