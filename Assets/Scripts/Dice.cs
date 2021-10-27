using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Sprite[] _diceSides;
    private SpriteRenderer _spriteRenderer;
    private int _rolledNumber;

    [SerializeField] private Vector3 _offset;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _diceSides = Resources.LoadAll<Sprite>("Dice/");

        Vector3 desiredPosition = Camera.main.ScreenToWorldPoint(Interface.instance.rollButton.transform.position);
        Vector3 newPosition = new Vector3(desiredPosition.x, desiredPosition.y + _offset.y, 0);

        transform.position = newPosition;
    }

    public IEnumerator RollDice()
    {
        int randomDiceSide = 0;

        //loops through dice sides 10 times before choosing the final random value
        for (int i = 0; i <= 10; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            _spriteRenderer.sprite = _diceSides[randomDiceSide];

            yield return new WaitForSeconds(0.05f);
        }

        _rolledNumber = randomDiceSide + 1;

        GameManager.instance.MovePlayer(_rolledNumber);
    }
}
