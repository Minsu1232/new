using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalSpawnManager : MonoBehaviour
{
    public GameObject[] animals; // 가능한 동물 프리팹 배열
    public int maxAnimals = 4; // 필드에 존재할 수 있는 최대 동물 수
    private List<GameObject> spawnedAnimals = new List<GameObject>(); // 생성된 동물을 추적하는 리스트

    // Start is called before the first frame update
    void Start()
    {
        // 동물 소환을 관리하는 코루틴 시작
        StartCoroutine(ManageAnimalSpawning());
    }

    // 동물 소환 및 관리를 위한 코루틴
    IEnumerator ManageAnimalSpawning()
    {
        // 초기 동물 소환
        while (spawnedAnimals.Count < maxAnimals)
        {
            SpawnRandomAnimal();
            yield return new WaitForSeconds(0.5f); // 초기 소환 시 간격
        }

        // 주기적으로 동물 수 확인 및 소환
        while (true)
        {
            yield return new WaitForSeconds(10); // 10초마다 실행

            // 삭제된 동물 제거
            spawnedAnimals.RemoveAll(item => item == null);

            // 동물 수가 최대보다 적으면 추가 생성
            while (spawnedAnimals.Count < maxAnimals)
            {
                SpawnRandomAnimal();
                yield return new WaitForSeconds(0.5f); // 추가 소환 시 간격
            }
        }
    }

    // 무작위 동물 생성 및 배치
    void SpawnRandomAnimal()
    {
        // 동물 배열에서 무작위 인덱스 선택
        int randomIndex = Random.Range(0, animals.Length);
        // 무작위 위치 생성
        Vector3 randomPosition = GetRandomPosition(gameObject.transform.position, 30, 30);
        // 동물 프리팹 인스턴스화 및 위치 지정
        GameObject spawnedAnimal = Instantiate(animals[randomIndex], randomPosition, Quaternion.identity);
        // 생성된 동물을 리스트에 추가
        spawnedAnimals.Add(spawnedAnimal);
    }

    // 지정된 중심에서 무작위 위치 계산
    Vector3 GetRandomPosition(Vector3 center, float width, float length)
    {
        // 지정된 범위 내에서 무작위 X 및 Z 좌표 생성
        float randomX = Random.Range(-width / 2, width / 2);
        float randomZ = Random.Range(-length / 2, length / 2);
        // 높이 값은 지형에서 샘플링하고, 1.2만큼 낮춤
        float yValue = Terrain.activeTerrain.SampleHeight(center + new Vector3(randomX, 0, randomZ)) - 1.2f;
        // 계산된 좌표로 새 위치 벡터 생성
        return new Vector3(randomX, yValue, randomZ) + center;
    }

    // Gizmos를 사용하여 Unity 에디터에서 스폰 영역을 시각적으로 표시
    private void OnDrawGizmos()
    {
        // Gizmos 색상 설정
        Gizmos.color = Color.green;
        // 스폰 영역의 중심 지점
        Vector3 center = gameObject.transform.position;
        // 스폰 영역의 크기 정의
        Vector3 size = new Vector3(30, 0.1f, 30);
        // 와이어 프레임 큐브로 스폰 영역 표시
        Gizmos.DrawWireCube(center, size);
    }
}
