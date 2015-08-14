using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;


public class MainLogic : MonoBehaviour {
	public Text dmgtext;
	public GameObject countdown; //3 2 1 세는 카운트 이미지
	public GameObject hpgauge; //HP 게이지바의 고정된 부분
	public GameObject hpgreen; //HP 게이지바의 줄어드는 초록색 부분
	public GameObject ask; //해치웠나? 하고 물어보는 말풍선

	int damageSum1; //1플레이어-전체 게임에서 누적된 피해량
	int damage1; //1플레이어-이번 페이즈에 누적된 피해량
	int damageSum2; //2플레이어-전체 게임에서 누적된 피해량
	int damage2; //2플레이어-이번 페이즈에 누적된 피해량

	int lastAttack; //마지막으로 공격한 플레이어
	int winner; //승리한 플레이어

	int bossMaxHp; //보스의 최대 체력
	int bossHp; //보스의 현재 체력
	int bossKillHP=200; //해치웠나? 나올때 보스가 죽는 체력 (해치웠나 없이 보스가 죽는 체력은 0)

	int timer; //다음 해치웠나? 까지 남은 시간 (0이 되면 발생)
	public int timerSum; //누계 타이머 (이펙트 부분에서 사용되므로 public으로 설정)
	int count3=30; //3 하는 이미지가 튀어나오는 시간
	int count2=20; //2 하는 이미지가 튀어나오는 시간
	int count1=10; //1 하는 이미지가 튀어나오는 시간

	int timerMinRange=200; //해치웠나 타이머 설정시 최소 시간
	int timerMaxRange=301; //해치웠나 타이머 설정시 최대 시간

	void BossHpUpdate(){
		if (bossHp>=0) hpgreen.transform.localScale = new Vector3 ((float)(bossHp)/(float)(bossMaxHp), 1, 1); //HP 게이지바 크기 설정
		else hpgreen.transform.localScale = new Vector3 (0, 1, 1); //HP 게이지바 크기 설정
		dmgtext.text = "보스 남은체력 (프레임당 5씩 닳게 설정함)" + Convert.ToString (bossHp);
	}

	void Ending(int hero){
		//hero 값이 1이냐 2냐에 따라 엔딩 씬으로 넘어가기
	}

	public void DamageHp(int n, int atk){
		lastAttack = atk; //마지막으로 공격한 플레이어 설정
		if (atk == 1) { //공격 플레이어가 1일때
			damageSum1=damageSum1+n; //1플레이어 전체 피해 누적
			damage1=damage1+n; //1플레이어 이번 페이즈 피해 누적
		} 
		else if (atk == 2) { //공격 플레이어가 2일때
			damageSum2=damageSum2+n; //2플레이어 전체 피해 누적
			damage2=damage2+n; //2플레이어 이번 페이즈 피해 누적
		}
		bossHp = bossHp - n; //보스 HP 감소
		BossHpUpdate(); //보스 HP띄우는 UI 갱신
		if (bossHp < 0) { //보스 체력이 0이 되어 강제종료
			System.Threading.Thread.Sleep(3000); //3초 대기
			if (damageSum1 < damageSum2) { //2플레이어가 누적 딜 높음
				winner=1; //1플레이어 승리
			} 
			else { //1플레이어가 누적 딜 높음
				winner=2; //2플레이어 승리
			}
			Ending(winner); //엔딩 호출
		}
	}

	void Hatchwotna(){
		//캐릭터 자세 변경하는거 호출할것
		if (damage1 < damage2) { //1플레이어가 해치웠나? 물어봄
			ask.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Interface/ask1"); //플레이어 1을 향하는 말풍선 (경로는 나중에 이미지에 맞게 수정할 것)
			ask.transform.position = new Vector3 (0, 0, 0); //해치웠나? 이미지 위치 설정 (지금은 화면 중앙)
		} 
		else { //2플레이어가 해치웠나? 물어봄
			ask.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Interface/ask2"); //플레이어 2를 향하는 말풍선 (경로는 나중에 이미지에 맞게 수정할 것)
			ask.transform.position = new Vector3 (0, 0, 0); //해치웠나? 이미지 위치 설정 (지금은 화면 중앙)
		}
		System.Threading.Thread.Sleep(3000); //3초 대기
		if (bossHp <= bossKillHP){ //보스 체력이 해치웠나? 할때 죽는 체력 이하일 때
			if (damage1 < damage2) { //1플레이어가 해치웠나? 물어봄
				winner=2; //2플레이어 승리
			} 
			else { //2플레이어가 해치웠나? 물어봄
				winner=1; //1플레이어 승리
			}
			Ending(winner); //엔딩 호출
		}
		timer=UnityEngine.Random.Range(timerMinRange, timerMaxRange); //다음 해치웠나? 타이머 시간 설정
		damage1 = 0; //1플레이어-이번 페이즈에 누적된 피해량 초기화
		damage2 = 0; //2플레이어-이번 페이즈에 누적된 피해량 초기화
	}

	// Use this for initialization
	void Start () {
		timerSum = 0; //누적 타이머 초기화
		damageSum1 = 0; //누적 피해 초기화1
		damage1 = 0; //현재 피해 초기화1
		damageSum2 = 0; //누적 피해 초기화2
		damage2 = 0; //현재 피해 초기화2
		bossMaxHp = 1200; //보스 최대 체력 초기화
		bossHp = bossMaxHp; //보스 현재 체력 초기화
		timer=UnityEngine.Random.Range(timerMinRange, timerMaxRange); //다음 해치웠나? 타이머 시간 설정
	}
	
	// Update is called once per frame
	void Update () {
		if (timer>0){
			timer--; //프레임마다 타이머 감소
			timerSum++; //프레임마다 누적 타이머 증가
			DamageHp(5, 1);
		}

		if (timer == count3) {
			countdown.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Interface/count3"); //카운트 이미지 3으로 변경 (경로는 나중에 이미지에 맞게 수정할 것)
			countdown.transform.position = new Vector3 (0, 0, 0); //카운트 이미지 위치 설정 (지금은 화면 중앙)
		} 
		else if (timer == count2) {
			countdown.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Interface/count2"); //카운트 이미지 2로 변경 (경로는 나중에 이미지에 맞게 수정할 것)
		} 
		else if (timer == count1) {
			countdown.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Interface/count1"); //카운트 이미지 1로 변경 (경로는 나중에 이미지에 맞게 수정할 것)
		} 
		else if (timer == 0) {
			countdown.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
			//Hatchwotna(); //해치웠나? 함수 호출
		}
	}
}
