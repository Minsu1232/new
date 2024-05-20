using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalSpawnManager : MonoBehaviour
{
    public GameObject[] animals; // ������ ���� ������ �迭
    public int maxAnimals = 4; // �ʵ忡 ������ �� �ִ� �ִ� ���� ��
    private List<GameObject> spawnedAnimals = new List<GameObject>(); // ������ ������ �����ϴ� ����Ʈ

    // Start is called before the first frame update
    void Start()
    {
        // ���� ��ȯ�� �����ϴ� �ڷ�ƾ ����
        StartCoroutine(ManageAnimalSpawning());
    }

    // ���� ��ȯ �� ������ ���� �ڷ�ƾ
    IEnumerator ManageAnimalSpawning()
    {
        // �ʱ� ���� ��ȯ
        while (spawnedAnimals.Count < maxAnimals)
        {
            SpawnRandomAnimal();
            yield return new WaitForSeconds(0.5f); // �ʱ� ��ȯ �� ����
        }

        // �ֱ������� ���� �� Ȯ�� �� ��ȯ
        while (true)
        {
            yield return new WaitForSeconds(10); // 10�ʸ��� ����

            // ������ ���� ����
            spawnedAnimals.RemoveAll(item => item == null);

            // ���� ���� �ִ뺸�� ������ �߰� ����
            while (spawnedAnimals.Count < maxAnimals)
            {
                SpawnRandomAnimal();
                yield return new WaitForSeconds(0.5f); // �߰� ��ȯ �� ����
            }
        }
    }

    // ������ ���� ���� �� ��ġ
    void SpawnRandomAnimal()
    {
        // ���� �迭���� ������ �ε��� ����
        int randomIndex = Random.Range(0, animals.Length);
        // ������ ��ġ ����
        Vector3 randomPosition = GetRandomPosition(gameObject.transform.position, 30, 30);
        // ���� ������ �ν��Ͻ�ȭ �� ��ġ ����
        GameObject spawnedAnimal = Instantiate(animals[randomIndex], randomPosition, Quaternion.identity);
        // ������ ������ ����Ʈ�� �߰�
        spawnedAnimals.Add(spawnedAnimal);
    }

    // ������ �߽ɿ��� ������ ��ġ ���
    Vector3 GetRandomPosition(Vector3 center, float width, float length)
    {
        // ������ ���� ������ ������ X �� Z ��ǥ ����
        float randomX = Random.Range(-width / 2, width / 2);
        float randomZ = Random.Range(-length / 2, length / 2);
        // ���� ���� �������� ���ø��ϰ�, 1.2��ŭ ����
        float yValue = Terrain.activeTerrain.SampleHeight(center + new Vector3(randomX, 0, randomZ)) - 1.2f;
        // ���� ��ǥ�� �� ��ġ ���� ����
        return new Vector3(randomX, yValue, randomZ) + center;
    }

    // Gizmos�� ����Ͽ� Unity �����Ϳ��� ���� ������ �ð������� ǥ��
    private void OnDrawGizmos()
    {
        // Gizmos ���� ����
        Gizmos.color = Color.green;
        // ���� ������ �߽� ����
        Vector3 center = gameObject.transform.position;
        // ���� ������ ũ�� ����
        Vector3 size = new Vector3(30, 0.1f, 30);
        // ���̾� ������ ť��� ���� ���� ǥ��
        Gizmos.DrawWireCube(center, size);
    }
}
