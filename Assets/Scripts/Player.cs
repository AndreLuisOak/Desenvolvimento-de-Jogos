using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Velocidade = 5f;
    private Rigidbody2D rb;
    public float ForcaPulo = 10f;
    public bool estaPulando;
    public bool puloDuplo;

    private Animator animator;
    private float movimentoHorizontal;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update(){
        movimentoHorizontal = Input.GetAxis("Horizontal");
        movimentacao();
        Pular();
        AtualizarAnimacao();
    }

    void movimentacao(){
        Vector3 movimento = new Vector3(movimentoHorizontal, 0f, 0f);
        transform.position += movimento * Time.deltaTime * Velocidade;

        if (movimentoHorizontal > 0f){
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (movimentoHorizontal < 0f){
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    void Pular(){
        if (Input.GetButtonDown("Jump")){
            if (!estaPulando){
                rb.AddForce(new Vector2(0f, ForcaPulo), ForceMode2D.Impulse);
                puloDuplo = true;
                estaPulando = true;
            }
            else if (puloDuplo){
                rb.AddForce(new Vector2(0f, ForcaPulo * 2f), ForceMode2D.Impulse);
                puloDuplo = false;
            }
        }
    }

    void AtualizarAnimacao(){
        animator.SetBool("Walk", movimentoHorizontal != 0 && !estaPulando);
        animator.SetBool("Jump", estaPulando);
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.layer == 6){
            estaPulando = false;
        }

        if (col.gameObject.CompareTag("Spike")){
            GameController.instance.GameOver();
            Destroy(gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D col){
        if (col.gameObject.layer == 6)
        {
            estaPulando = true;
        }
    }

}
