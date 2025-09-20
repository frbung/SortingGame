using System.Collections;
using SortingLib;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    public GameObject prefab; // Assign this in the Inspector

    public int[] numbers;


    // Start is called before the first frame update
    void Start()
    {
        numbers = GameUtil.GenerateList(10);
        StartCoroutine(SpawnCubes());
    }


    // Update is called once per frame
    void Update()
    {
    }


    IEnumerator SpawnCubes()
    {
        foreach (var number in numbers)
        {
            var spawnPosition = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(2, 15), Random.Range(-0.5f, 0.5f));
            var spawnRotation = Quaternion.Euler(0, number, number);
            var obj = Instantiate(prefab, spawnPosition, spawnRotation);

            obj.tag = number.ToString();
            obj.name = $"Cube {number}";
            var objScript = obj.GetComponent<NumberObjectScript>();
            objScript.number = number;

            yield return new WaitForSeconds(0.5f);
        }
    }
}
