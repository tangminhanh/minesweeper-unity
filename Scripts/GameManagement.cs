using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManagement : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform gameHolder;
    private List<Tile> tiles = new();
    private int width;
    private int height;
    private int numMines;

    private readonly float tileSize = 0.5f;
    public int mineCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        CreateGameBoard(7, 7, 5);
        ResetGameState();
        //// Choose a specific tile to start with (e.g., the tile at index 0)
        //Tile tileToStartWith = tiles[0];

        //// Call the RevealSurroundingTiles method for the chosen tile
        //RevealSurroundingTiles(tileToStartWith);
    }

    // Update is called once per frame
    public void CreateGameBoard(int width, int height, int numMines)
    {
        this.width = width;
        this.height = height;
        this.numMines = numMines;

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                Transform tileTransform = Instantiate(tilePrefab);
                tileTransform.parent = gameHolder;
                float xIndex = col - ((width - 1) / 2.0f);
                float yIndex = row - ((height - 1) / 2.0f);
                tileTransform.localPosition = new Vector2(xIndex * tileSize, yIndex * tileSize);
                Tile tile = tileTransform.GetComponent<Tile>();
                tiles.Add(tile);
                tile.gameManagement = this; 
            }
        }
    }
    private void ResetGameState()
    {
        // Randomly shuffle the tile positions to get indices for mine positions.
        int[] minePositions = Enumerable.Range(0, tiles.Count).OrderBy(x => Random.Range(0.0f, 1.0f)).ToArray();

        // Set mines at the first numMines positions.
        for (int i = 0; i < numMines; i++)
        {
            int pos = minePositions[i];
            tiles[pos].isMine = true; 
        }

        // Update all the tiles to hold the correct number of mines.
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].mineCount = HowManyMines(i); //for revealTiles
        }
    }
    private int HowManyMines(int location)
    {
        int count = 0;
        foreach (int pos in GetNeighbours(location))
        {
            if (tiles[pos].isMine)
            {
                count++;
            }
        }
        return count;
    }

    //get 8 tiles around (3x3 square)
    private List<int> GetNeighbours(int pos)
    {
        List<int> neighbours = new();
        int row = pos / width;
        int col = pos % width;
        // (0,0) is bottom left.
        if (row < (height - 1))
        {
            neighbours.Add(pos + width); // North
            if (col > 0)
            {
                neighbours.Add(pos + width - 1); // North-West
            }
            if (col < (width - 1))
            {
                neighbours.Add(pos + width + 1); // North-East
            }
        }
        if (col > 0)
        {
            neighbours.Add(pos - 1); // West
        }
        if (col < (width - 1))
        {
            neighbours.Add(pos + 1); // East
        }
        if (row > 0)
        {
            neighbours.Add(pos - width); // South
            if (col > 0)
            {
                neighbours.Add(pos - width - 1); // South-West
            }
            if (col < (width - 1))
            {
                neighbours.Add(pos - width + 1); // South-East
            }
        }
        return neighbours;
    }

    public void RevealSurroundingTiles(Tile tile)
    {
        // Get the position of the tile in the list
        int location = tiles.IndexOf(tile);

        // Iterate through neighboring tiles
        foreach (int pos in GetNeighbours(location))
        {
            if (!tiles[pos].IsRevealed())
            {
                tiles[pos].Reveal();
            }
            // Get and put sprite the neighboring tile
        }
    }

}
