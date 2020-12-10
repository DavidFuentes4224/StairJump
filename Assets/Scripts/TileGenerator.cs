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
    public Parallaxer Background;

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
            var direction = (-1 + chance) * Settings.DISTANCE;
            currentPosition += new Vector2(direction, Settings.HEIGHT);
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
                var turnDirection = (-1 + chance) * Settings.DISTANCE;
                tile.position = m_nextPos;
                tileRef.ResetTarget();
                tile.gameObject.SetActive(true);
                m_nextPos += new Vector3(turnDirection, Settings.HEIGHT);
            }
            else
            {
                if (tile.position.y < (GameStateManager.PlayerRef.transform.position.y - MaxDistanceDown))
                {
                    tile.gameObject.SetActive(false);
                }
            }
            tileRef.UpdateTarget(Settings.DISTANCE * direction);
        }
        m_nextPos += new Vector3(Settings.DISTANCE * direction, -Settings.HEIGHT);
        Background.UpdateTargetOffset(Settings.DISTANCE * direction);
    }

    public int GetShouldTurn()
    {
        return Random.Range(0f, 1f) < m_turnChance ? 2 : 0;
    }
}
