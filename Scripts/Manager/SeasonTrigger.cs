using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonTrigger : MonoBehaviour {

    public PlayerStatus.Season thisAreaSeason = PlayerStatus.Season.none;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerStatus.set_inSeason(thisAreaSeason);
            //Debug.Log(PlayerStatus.get_inSeason());
        }
    }
}
