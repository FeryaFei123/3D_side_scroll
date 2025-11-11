using UnityEngine;

public class followCamera : MonoBehaviour
{
    public Transform player; // ลาก Player GameObject มาใส่ในช่องนี้

    // นี่คือ "ระยะห่าง" ที่กล้องจะเว้นไว้จาก Player
    // เราจะคำนวณค่านี้อัตโนมัติจากตำแหน่งเริ่มต้นใน Scene
    private Vector3 offset;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("ยังไม่ได้กำหนด Player ให้กับกล้อง!");
            return;
        }

        // คำนวณและจำระยะห่างระหว่างกล้องกับ Player
        // โดยยึดจากตำแหน่งที่เราวางไว้ใน Scene ตอนเริ่มเกม
        offset = transform.position - player.position;
    }

    // ใช้ LateUpdate เพื่อป้องกันอาการสั่น (Jitter)
    // เพราะมันจะทำงานหลังจากที่ Player (Physics) เคลื่อนที่เสร็จแล้ว
    void LateUpdate()
    {
        if (player != null)
        {
            // ตั้งค่าตำแหน่งกล้อง = ตำแหน่งใหม่ของ Player + ระยะห่างที่คำนวณไว้
            // นี่คือการ "Hard Follow" (ตามติด) โดยไม่มีการหน่วง (Lerp) ใดๆ
            transform.position = player.position + offset;
        }
    }
}
