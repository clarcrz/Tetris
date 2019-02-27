using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{

    float fall = 0;
    public static float fallSpeed = 1f;

    float downlastStep, downTimeBetweenSteps = 0.15f;
    float keyLastStep, keyTimeBetweenSteps = 0.3f;

    public bool allowRotation = true;
    public bool limitRotation = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckUserInput();
    }

    /*
     * Checks user key input and moves tetromino downward
     * and updates container grid
     */
    void CheckUserInput ()
    {
        // right arrow user input
        if (Input.GetKey(KeyCode.RightArrow)) {

            if (Time.time - keyLastStep > keyTimeBetweenSteps) {
                keyLastStep = Time.time;
            
                // Shift tetromino position to the right
                transform.position += new Vector3(1, 0, 0);

                // if new position is valid update container grid
                // else remove changes in position
                if (CheckIsValidPosition()) {
                    FindObjectOfType<Game>().UpdateGrid(this);
                } else {
                    transform.position += new Vector3(-1, 0, 0);
                }
            }
            // left arrow user input
        } else if (Input.GetKey(KeyCode.LeftArrow)) {

            if (Time.time - keyLastStep > keyTimeBetweenSteps) {
                keyLastStep = Time.time;

                // Shift tetromino position to the left
                transform.position += new Vector3(-1, 0, 0);

                // if new position is valid update container grid
                // else remove changes in position 
                if (CheckIsValidPosition()) {
                    FindObjectOfType<Game>().UpdateGrid(this);
                } else {
                    transform.position += new Vector3(1, 0, 0);
                }
            }
            // up arrow user input
        } else if (Input.GetKey(KeyCode.UpArrow)) {

            if (Time.time - keyLastStep > keyTimeBetweenSteps) {
                keyLastStep = Time.time;

                // if piece can be rotated
                if (allowRotation) {
                    // if piece can only be rotated by 180 deg (Long, Z, S pieces)
                    if (limitRotation) {
                        if (transform.rotation.eulerAngles.z >= 90) {
                            transform.Rotate(0, 0, -90);
                        } else {
                            transform.Rotate(0, 0, 90);
                        }
                        // if piece can be rotated by 90 deg ( J, L, T pieces)
                    } else {
                        transform.Rotate(0, 0, 90);
                    }

                    // if position is valid update container grid
                    if (CheckIsValidPosition()) {
                        FindObjectOfType<Game>().UpdateGrid(this);
                        // else return to original position
                    } else {
                        if (limitRotation) {
                            if (transform.rotation.eulerAngles.z >= 90) {
                                transform.Rotate(0, 0, -90);
                            } else {
                                transform.Rotate(0, 0, 90);
                            }
                        } else {
                            transform.Rotate(0, 0, -90);
                        }
                    }
                }

            }
            // down arrow user input and falling piece timer
        } else if (Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= fallSpeed) {

            if (Time.time- downlastStep > downTimeBetweenSteps) {
                downlastStep = Time.time;
                // move piece downward
                transform.position += new Vector3(0, -1, 0);
                // if position is valid update container grid
                // else return to original position, stop movement and spawn next piece
                if (CheckIsValidPosition()) {
                    FindObjectOfType<Game>().UpdateGrid(this);
                } else {
                    transform.position += new Vector3(0, 1, 0);
                    FindObjectOfType<Game>().DeleteRow();
                    if (FindObjectOfType<Game>().CheckIsAboveGrid(this)) {
                        FindObjectOfType<Game>().GameOver();
                    }
                    enabled = false;
                    FindObjectOfType<Game>().SpawnNextTetromino();
                }
            }

            fall = Time.time;

        } 
    }

    /*
     * Checks grid border and occupied spaces in grid
     * to determine if next position is valid
     */
    bool CheckIsValidPosition ()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);

            if (FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false)
            {
                return false;
            }

            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null 
                && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform) {
                return false;
            }
        }

        return true;
    }

    /*
     * Get fall speed for level up calculation
     */
    public float GetFallSpeed()
    {
        return fallSpeed;
    }

    /*
     * Set fall speed after level up
     */
    public void SetFallSpeed (float fall_speed)
    {
        fallSpeed = fall_speed;
    }
}
