using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InboxListItem : ListItem
{
    [SerializeField] private Image Image_Sender;
    [SerializeField] private TextMeshProUGUI Text_Sender;
    [SerializeField] private TextMeshProUGUI Text_Message;

    public void Setup(Sprite _sprite, string _sender, string _message)
    {
        Image_Sender.sprite = _sprite;
        Text_Sender.text = _sender;
        Text_Message.text = _message;
    }
}
