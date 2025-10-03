## 개요
- 윷놀이 + AOS 게임
- [세부 기획](https://easy-h.notion.site/2024-1355e129e5ff8050afa4e1b23c7b184e?pvs=74)

## 개발도구
- Unity 6000.0.58f2
	- https://docs.unity3d.com/Manual/PostProcessingOverview.html
- Unity Network GameObject: 멀티 플레이 구현에 사용되었습니다.
- NewtonJson: Json Parsing에 사용되었습니다.
- EasyH: UI, 언어 시스템에 사용되었습니다.
- BKTools: 주사위 값 획득 시스템에 사용되었습니다.

## 기능 및 계획
- [x] 경로 기반 보드판 이동
	- EasyH.Gaming.PathBased 모듈 사용
- [x] 기물
	- [x] 기물이 플레이트에 도착했을 때 이벤트
    	- [x] 공격력 획득
    	- [x] 수비력 획득
    	- [x] 체력 획득
    	- [x] 전투 시작
  	- [x] 기물의 겹침 처리
    	- [x] 업기
    	- [x] 원래 기물을 집으로 돌려보낸 후 한번 더 말을 옮길 기회
- [x] 주사위
	- BKToos.Dice 모듈 사용
	- [x] 물리적 주사위(D6)
  	- [x] 룰렛
  	- [x] 다트
	- [ ] Unity Network GameObject로 네트워크 환경에서의 주사위 값 동기화
- [x] 전투 시스템
	- [x] 전투 UI
	- [x] 오브젝트와의 전투 
- [x] 턴제 게임 진행: EasyH.Gaming.TurnBased를 사용하였습니다.
	- [x] Unity Network GameObject로 네트워크 환경에서의 멀티플레이 구현
- [x] 아이템
	- [x] 인벤토리
	- [x] 주사위 사용 기회 추가
	- [x] 기물 이동 거리 증가
- [ ] AI
	- [x] A* 알고리즘을 이용한 AI
  	- [ ] API를 이용한 AI
- [x] 언어 변경

## 라이선스
- 이 프로젝트는 MIT 라이센스에 따라 배포됩니다.
- 자세한 내용은 LICENSE 파일을 참고해주세요.

## 연락
- skysea001010@gmail.com

## 참고 자료
- https://gofogo.tistory.com/64
