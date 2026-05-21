using System.Collections;
using UnityEngine;

public class GeneratorLogic : MonoBehaviour
{
    public GameObject rotatingPiece;
    public float rotationSpeed = 90f;
    public float moveUpDistance = 2f;
    public float moveUpDuration = 2f;
    public float shootDownDuration = 0.5f;
    public bool isdead = false;

    private bool isMoving = false;
    private BaseScript baseScript;
    private GameObject Tbase;

    private void Awake()
    {
        Tbase = GameObject.FindGameObjectWithTag("Base");
        if (Tbase != null)
        {
            baseScript = Tbase.GetComponent<BaseScript>();
            baseScript?.AddGeneratorRadius();
        }
    }

    private void OnDestroy()
    {
        if (isdead)
        {
            baseScript?.RemoveGeneratorRadius();
        }
    }

    private void Update()
    {
        if (rotatingPiece == null)
        {
            return;
        }

        rotatingPiece.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        if (!isMoving)
        {
            StartCoroutine(MovePiece());
        }
    }

    private IEnumerator MovePiece()
    {
        isMoving = true;

        if (rotatingPiece == null)
        {
            isMoving = false;
            yield break;
        }

        Vector3 startPosition = rotatingPiece.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * moveUpDistance;

        for (float t = 0; t < 1; t += Time.deltaTime / moveUpDuration)
        {
            rotatingPiece.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        rotatingPiece.transform.position = endPosition;

        for (float t = 0; t < 1; t += Time.deltaTime / shootDownDuration)
        {
            rotatingPiece.transform.position = Vector3.Lerp(endPosition, startPosition, t);
            yield return null;
        }

        BaseScript.AddCredits(12);
        if (EconomyService.Instance != null)
        {
            print(EconomyService.Instance.Credits);
        }

        rotatingPiece.transform.position = startPosition;
        isMoving = false;
    }
}
