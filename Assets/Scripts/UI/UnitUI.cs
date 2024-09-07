using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private UnitCard _card;

    public List<UnitCard> UnitCards { get; private set; }

    private void Awake()
    {
        UnitCards = new List<UnitCard>();
    }

    public void CreateChildElements(IEnumerable<Unit> units)
    {
        int index = 0;

        foreach (var unit in units)
        {
            var card = Instantiate(_card, gameObject.transform);

            card.SetUnit(unit);
            card.SetNumber(++index);
            UnitCards.Add(card);
        }
    }

    public void RemoveChildsElement()
    {
        foreach (var card in UnitCards)
        {
            if (card != null)
            {
                Destroy(card.gameObject);

            }
        }

        UnitCards.Clear();
    }

    public void DeselecAll()
    {
        foreach (var card in UnitCards)
        {
            if (!card.IsDestroy)
            {
                card.Deselect();
                card.Unit.IsSelected = false;
            }
        }
    }
}
