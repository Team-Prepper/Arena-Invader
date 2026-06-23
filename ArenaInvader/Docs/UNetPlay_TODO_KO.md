# _UNetPlay TODO

작성일: 2026-06-23

## 완료

- [x] `UNetMatchInfo` 이름 변경 시 `IsAI` 유지
- [x] `UNetMatchInfo` 이벤트 구독/해제 생명주기 정리
- [x] `UNetTurnSystem` 0팀, null 조건식, 빈 팀 순회 방어
- [x] `UNetPlayground` 로컬 플레이어 선택을 `clientId` 기반으로 1차 보강

## 다음 할 일

### P3

- [x] `UNetMemberState`의 ownership 기준 턴 시작/종료 흐름 정리
- [x] `UNetStatus`의 `IsOwner` 기반 스탯 변경을 서버 확정 구조로 정리
- [x] `UNetCharacterController` 구조를 로컬 버전과 더 가깝게 정리
- [x] 플레이어 외 네트워크 오브젝트의 ownership/권한 기준 정리
- [x] 인벤토리 구조 정리 이후 불필요해진 RPC/UI 동기화 코드 정리

## 메모

- [x] 캐릭터 스폰 ownership을 명시적으로 연결할지 검토
