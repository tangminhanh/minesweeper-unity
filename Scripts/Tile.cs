using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [Header("Tile Sprites")]
    [SerializeField] private List<Sprite> revealTiles; //X and ?
    [SerializeField] private Sprite unclickedTile;
    [SerializeField] private Sprite clickedTiles; //empty
    [SerializeField] private Sprite mineTile; //reveal after game end
    [SerializeField] private Sprite mineHitTile; //reveal when clicked

    //[Header("GM set via code")]
    public GameManagement gameManagement;

    private SpriteRenderer spriteRenderer;
    //public bool flagged = false;
    public bool active = true;
    public bool isMine = false;
    public int mineCount = 0;


    void Awake()
    {
        // This should always exist due to the RequireComponent helper.
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseOver()
    {
        // If it hasn't already been pressed.
        if (active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // If left click reveal the tile contents.
                ClickedTile();
                gameManagement.RevealSurroundingTiles(this);

            }
        }
    }

    //public void SetTileSprite(Sprite sprite)
    //{
    //    spriteRenderer.sprite = sprite;
    //}

    public void ClickedTile()
    {
        // Don't allow left clicks on flags.
        if (active)
        {
            // Ensure it can no longer be pressed again.
            active = false;
            if (isMine)
            {
                // Game over :( sad
                spriteRenderer.sprite = mineHitTile;
                //gameManager.GameOver();
            }
            else
            {
                // It was a safe click, empty tile
                spriteRenderer.sprite = clickedTiles;
                //// Whenever we successfully make a change check for game over.
                //gameManager.CheckGameOver();
            }
        }
    }

    public void Reveal()
    {
        if (!isMine)
        {
            spriteRenderer.sprite = revealTiles[0];
        }
        else
        {
            spriteRenderer.sprite = revealTiles[1];
        }
    }

    public bool IsRevealed()
    {
        // Check if the tile is already revealed
        return spriteRenderer.sprite != unclickedTile;
    }
}



