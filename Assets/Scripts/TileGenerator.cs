using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_tiles;
    [SerializeField]
    private float m_turnChance;
    [SerializeField]
    private Vector3 m_nextPos;

    public int MAXTILES;
    public Transform TilePrefab;
    public int MaxDistanceDown;
    public Background Background;

    private void Awake()
    {
        m_tiles = new Transform[MAXTILES];
    }

    private void Start()
    {
        m_turnChance = 0.55f;
        GenerateTiles();
    }

    private void Update()
    {
        Debug.DrawLine(Vector3.zero, m_nextPos, Color.yellow);
    }

    private void GenerateTiles()
    {
        var currentPosition = new Vector2(0, 0);
        for(int i = 0; i < m_tiles.Length; i++)
        {
            var pooledObject = Instantiate(TilePrefab, currentPosition, Quaternion.identity);
            pooledObject.name = $"Platform{i}";
            pooledObject.gameObject.SetActive(true);

            m_tiles[i] = pooledObject;
            var chance = GetShouldTurn();
            var direction = (-1 + chance) *2;
            currentPosition += new Vector2(direction,1);
        }
        m_nextPos = currentPosition;
    }

    public void UpdateTiles(int direction)
    {
        for (int i = 0; i < m_tiles.Length; i++)
        {
            var tile = m_tiles[i];
            var tileRef = tile.GetComponent<Tile>();
            if (!tile.gameObject.active)
            {
                var chance = GetShouldTurn();
                var turnDirection = (-1 + chance) * 2;
                tile.position = m_nextPos;
                tileRef.ResetTarget();
                tile.gameObject.SetActive(true);
                m_nextPos += new Vector3(turnDirection, 1);
            }
            else
            {
                if (tile.position.y < (GameStateManager.PlayerRef.transform.position.y - MaxDistanceDown))
                {
                    tile.gameObject.SetActive(false);
                }
            }
            tileRef.UpdateTarget(2 * direction);
        }
        m_nextPos += new Vector3(2 * direction, -1);
        Background.UpdateOffset(2 * direction);
    }

    public int GetShouldTurn()
    {
        return Random.Range(0f, 1f) < m_turnChance ? 2 : 0;
    }
}
