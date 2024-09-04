using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyDefence
{
    //터렛을 관리하는 클래스
    public class Turret : MonoBehaviour
    {
        //필드
        #region Variable
        private Transform target;       //타겟
        public Transform partToRotate;  //터렛 회전을 관리하는 오브젝트

        public float attackRange = 7f;  //공격 범위

        //target 찾기 타이머
        public float serachTimer = 0.5f;
        private float countdown = 0f;

        //회전
        public float turnSpeed = 10f;
        #endregion


        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            //0.5초마다 타겟 찾기 실행
            if (countdown <= 0f)
            {
                //타이머 실행문
                UpdateTarget();

                //타이머 초기화
                countdown = serachTimer;
            }
            countdown -= Time.deltaTime;

            //타겟을 찾지 못했으면
            if (target == null)
            {
                return;
            }

            //타겟을 향해 총구를 회전
            Vector3 dir = target.position - partToRotate.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, turnSpeed * Time.deltaTime).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        }

        //적들중 공격할 target 적을 찾는다
        void UpdateTarget()
        {
            //Debug.Log("UpdateTarget=======");
            //"Enemy" 태그를 가진 오브젝트들 객체 가져오기
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            //거리가 가장 가까운 적 찾기
            float minDistance = float.MaxValue; //최소거리
            GameObject minEnemy = null;         //최소거리에 있는 적

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(this.transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minEnemy = enemy;
                }
            }

            //가장 가까운 적이 공격 범위 안에 있는지 체크
            if(minEnemy != null && minDistance < attackRange)
            {
                target = minEnemy.transform;
            }
            else
            {
                target = null;
            }
        }

        //공격범위 확인용 기즈모 그리기
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, attackRange);
        }
    }
}