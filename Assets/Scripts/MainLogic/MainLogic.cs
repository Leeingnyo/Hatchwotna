using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;


public class MainLogic : MonoBehaviour {
	AudioSource sound;

	public Text dmgtext;
	public GameObject countdown31; //3 2 1 세는 카운트 이미지
	public GameObject countdown21; //3 2 1 세는 카운트 이미지
	public GameObject countdown11; //3 2 1 세는 카운트 이미지
	public GameObject countdown32; //3 2 1 세는 카운트 이미지
	public GameObject countdown22; //3 2 1 세는 카운트 이미지
	public GameObject countdown12; //3 2 1 세는 카운트 이미지
	public GameObject hpgauge; //HP 게이지바의 고정된 부분
	public GameObject hpgreen; //HP 게이지바의 줄어드는 초록색 부분
	public GameObject ask1; //해치웠나? 하고 물어보는 말풍선
	public GameObject ask2; //해치웠나? 하고 물어보는 말풍선
	public GameObject demon; //HP 게이지바의 고정된 부분
	public GameObject demonAngry; //HP 게이지바의 고정된 부분
	public GameObject demonDead; //HP 게이지바의 고정된 부분

	int damageSum1; //1플레이어-전체 게임에서 누적된 피해량
	int damage1; //1플레이어-이번 페이즈에 누적된 피해량
	int damageSum2; //2플레이어-전체 게임에서 누적된 피해량
	int damage2; //2플레이어-이번 페이즈에 누적된 피해량

	int lastAttack; //마지막으로 공격한 플레이어
	int winner; //승리한 플레이어

    public int targetPlayerNumber;

	int bossMaxHp; //보스의 최대 체력
	int bossHp; //보스의 현재 체력
	int bossKillHP=200; //해치웠나? 나올때 보스가 죽는 체력 (해치웠나 없이 보스가 죽는 체력은 0)

	float timer; //다음 해치웠나? 까지 남은 시간 (0이 되면 발생)
	public float timerSum; //누계 타이머 (이펙트 부분에서 사용되므로 public으로 설정)
	float count3=3f; //3 하는 이미지가 튀어나오는 시간
	float count2=2f; //2 하는 이미지가 튀어나오는 시간
	float count1=1f; //1 하는 이미지가 튀어나오는 시간
	int countstate=4;

	int timerMinRange=1400; //해치웠나 타이머 설정시 최소 시간
	int timerMaxRange=2001; //해치웠나 타이머 설정시 최대 시간
	public bool hatchWotnaState=false; //해치웠나 상태 false로 초기화
	float hatchWotnaTimer=0; //해치웠나 대기 타이머
	float demonTimer=0; //마왕 공격 및 회복 타이머

	void BossHpUpdate(){
		if ((float)(bossHp) / (float)(bossMaxHp) > 0.5f) {
			hpgreen.GetComponent<SpriteRenderer>().color = new Color(0, 0.9f, 0); //체력바 초록색 설정
		} 
		else if ((float)(bossHp) / (float)(bossMaxHp) > 0.2f) {
			hpgreen.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0.6f, 0); //체력바 주황색 설정
		} 
		else {
			hpgreen.GetComponent<SpriteRenderer>().color = new Color(0.9f, 0, 0); //체력바 빨강색 설정
		}
		if (bossHp>=0) hpgreen.transform.localScale = new Vector3 ((float)(bossHp)/(float)(bossMaxHp), 1, 1); //HP 게이지바 크기 설정
		else hpgreen.transform.localScale = new Vector3 (0, 1, 1); //HP 게이지바 크기 설정
		dmgtext.text = "보스 남은체력 (프레임당 5씩 닳게 설정함)" + Convert.ToString (bossHp)+" 타이머 : "+Convert.ToString (timer);
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
			countdown11.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
			countdown12.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
			countdown21.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
			countdown22.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
			countdown31.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
			countdown32.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
			demon.transform.position = new Vector3 (-5000, -5000, 0); //악마 이미지 위치 설정
			demonDead.transform.position = new Vector3 (-0.2f, 0.2f, 0); //악마 이미지 위치 설정
			FindObjectOfType<EffectManager> ().HideSmokes();
			hatchWotnaState = true; //공격 못하게 막음
			hatchWotnaTimer=3f;
			//System.Threading.Thread.Sleep(3000); //3초 대기
		}
	}

	void Hatchwotna(){
		hatchWotnaState = true; //공격 못하게 막음
		//캐릭터 자세 변경하는거 호출할것
		if (damage1 < damage2) { //1플레이어가 해치웠나? 물어봄
            targetPlayerNumber = 1;
			ask1.transform.position = new Vector3 (0, -3.5f, 0); //해치웠나? 이미지 위치 설정 (지금은 화면 중앙)
			sound=ask1.GetComponent<AudioSource>();
			sound.Play();
		} 
		else { //2플레이어가 해치웠나? 물어봄
            targetPlayerNumber = 2;
			ask2.transform.position = new Vector3 (0, -3.5f, 0); //해치웠나? 이미지 위치 설정 (지금은 화면 중앙)
			sound=ask2.GetComponent<AudioSource>();
			sound.Play();
		}
		hatchWotnaTimer=3f;
		if (bossHp <= bossKillHP){ //보스 체력이 해치웠나? 할때 죽는 체력 이하일 때
			demon.transform.position = new Vector3 (-5000, -5000, 0); //악마 이미지 위치 설정
			demonDead.transform.position = new Vector3 (-0.2f, 0.2f, 0); //악마 이미지 위치 설정
		}
		FindObjectOfType<EffectManager> ().HideSmokes();
		//System.Threading.Thread.Sleep(3000); //3초 대기
	}

	// Use this for initialization
	void Start () {
		demon.transform.position = new Vector3 (0, 0.6f, 0); //악마 이미지 위치 설정
		timerSum = 0; //누적 타이머 초기화
		damageSum1 = 0; //누적 피해 초기화1
		damage1 = 0; //현재 피해 초기화1
		damageSum2 = 0; //누적 피해 초기화2
		damage2 = 0; //현재 피해 초기화2
		bossMaxHp = 1200; //보스 최대 체력 초기화
		bossHp = bossMaxHp; //보스 현재 체력 초기화
		countstate = 4;
		timer=(float)(UnityEngine.Random.Range(timerMinRange, timerMaxRange))/100f; //다음 해치웠나? 타이머 시간 설정
		hpgreen.GetComponent<SpriteRenderer>().color = new Color(0, 0.9f, 0); //체력바 초록색 설정
	}
	
	// Update is called once per frame
	void Update () {
		if (demonTimer>0){
			demonTimer=demonTimer-Time.deltaTime; //프레임마다 타이머 감소
			bossHp=bossHp+Convert.ToInt32(Time.deltaTime*300.0f);
			if (bossHp>bossMaxHp) bossHp=bossMaxHp;
			BossHpUpdate(); //보스 HP띄우는 UI 갱신
			if (demonTimer<=0){
				demon.transform.position = new Vector3 (0, 0.6f, 0); //악마 이미지 위치 설정
				demonAngry.transform.position = new Vector3 (-5000, -5000, 0); //악마 이미지 위치 설정
			}
		}
		if (timer>0){
			timer=timer-Time.deltaTime; //프레임마다 타이머 감소
			timerSum=timerSum+Time.deltaTime; //프레임마다 누적 타이머 증가
		}
		if (hatchWotnaState == false) {
			if (timer <= 0) {
				countdown11.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
				countdown12.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
				Hatchwotna (); //해치웠나? 함수 호출
				countstate=4;
			} else if (timer <= count1 && countstate > 1) {
				countdown21.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
				countdown22.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
				if (damage1<damage2) countdown11.transform.position = new Vector3 (0, -3.5f, 0); //카운트 이미지 위치 설정 (지금은 화면 중앙)
				else countdown12.transform.position = new Vector3 (0, -3.5f, 0); //카운트 이미지 위치 설정 (지금은 화면 중앙)
				countstate = 1;
			} else if (timer <= count2 && countstate > 2) {
				countdown31.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
				countdown32.transform.position = new Vector3 (-5000, -5000, 0); //카운트 이미지 지우기 (안보이는곳으로 보냄)
				if (damage1<damage2) countdown21.transform.position = new Vector3 (0, -3.5f, 0); //카운트 이미지 위치 설정 (지금은 화면 중앙)
				else countdown22.transform.position = new Vector3 (0, -3.5f, 0); //카운트 이미지 위치 설정 (지금은 화면 중앙)
				countstate = 2;
			} else if (timer <= count3 && countstate > 3) {
				if (damage1<damage2) countdown31.transform.position = new Vector3 (0, -3.5f, 0); //카운트 이미지 위치 설정 (지금은 화면 중앙)
				else countdown32.transform.position = new Vector3 (0, -3.5f, 0); //카운트 이미지 위치 설정 (지금은 화면 중앙)
				countstate = 3;
			} 
		}

		if (hatchWotnaState == true) {
			if (hatchWotnaTimer>0) hatchWotnaTimer=hatchWotnaTimer-Time.deltaTime;
			else if (hatchWotnaTimer<=0){
				if (bossHp < 0) { //보스 체력이 0이 되어 강제종료
					if (damageSum1 < damageSum2) { //2플레이어가 누적 딜 높음
						winner=1; //1플레이어 승리
					} 
					else { //1플레이어가 누적 딜 높음
						winner=2; //2플레이어 승리
					}
					Ending(winner); //엔딩 호출
				}
				else{
					if (bossHp <= bossKillHP){ //보스 체력이 해치웠나? 할때 죽는 체력 이하일 때
						if (damage1 < damage2) { //1플레이어가 해치웠나? 물어봄
							winner=2; //2플레이어 승리
						} 
						else { //2플레이어가 해치웠나? 물어봄
							winner=1; //1플레이어 승리
						}
						Ending(winner); //엔딩 호출
					}
					else{
						demon.transform.position = new Vector3 (-5000, -5000, 0); //악마 이미지 위치 설정
						demonAngry.transform.position = new Vector3 (0, 0.3f, 0); //악마 이미지 위치 설정
						sound=demonAngry.GetComponent<AudioSource>();
						sound.Play();
                        FindObjectOfType<EffectManager>().ShowSmokes(); // 연기 일정량 재생성
						demonTimer=2f;
						timer=(float)(UnityEngine.Random.Range(timerMinRange, timerMaxRange))/100f;
						damage1 = 0; //1플레이어-이번 페이즈에 누적된 피해량 초기화
						damage2 = 0; //2플레이어-이번 페이즈에 누적된 피해량 초기화
						ask1.transform.position = new Vector3 (-5000, -5000, 0); //해치웠나? 이미지 지우기 (안보이는곳으로 보냄)
						ask2.transform.position = new Vector3 (-5000, -5000, 0); //해치웠나? 이미지 지우기 (안보이는곳으로 보냄)
						hatchWotnaState = false; //공격 가능하게 품
					}
				}

			}
		}
	}
}
