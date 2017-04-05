using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Prefab DB")]
public class PrefabDB : ScriptableObject {
	[SerializeField]
	private GameObject coin;
	public GameObject Coin { get { return coin; } }

	[SerializeField]
	private GameObject player;
	public GameObject Player { get { return player; } }

    [SerializeField]
    private GameObject bank;
    public GameObject Bank { get { return bank; } }
}
