# _UNetPlay 차이 메모

작성일: 2026-06-23

이 문서는 로컬 `ArenaInvader`와 `_UNetPlay` 구현 사이의 차이와 현재 남은 보완 포인트를 간단히 정리한 메모입니다.

## 이번에 맞춘 항목

1. `UNetMatchInfo`
- `PlayerInfor.IsAI`가 네트워크 데이터에 포함되도록 수정
- `SetPlayerIsAI()`가 실제로 동작하도록 서버 RPC 추가
- `SetMap()`이 잘못 `NetMatchDice`를 바꾸던 문제 수정

2. `UNetStatus`
- `HP`, `Money` 하한값 보정 추가
- `LevelUp()` 레벨 범위 clamp 추가
- `Atk`, `Dfs` 계산 시 안전한 레벨 인덱스 사용
- 사망 이벤트 중복 호출 방지 추가
- `AddDfs()` 누적 규칙을 로컬과 통일

## 아직 남아 있는 차이

1. `UNetCharacterController` 구조
- [UNetCharacterController.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/_UNetPlay/ArenaInvader/Character/UNetCharacterController.cs)
- 로컬 `PlayableCharacter`처럼 턴 상태 분리, 초기화 검증, 해제 정리가 부족합니다.

2. `UNetMemberState` 흐름
- [UNetMemberState.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/_UNetPlay/EasyH.Gaming.TurnBased/UNetMemberState.cs)
- 로컬 쪽과 이벤트 의미는 비슷하지만, 네트워크 소유권과 턴 전파 규칙을 별도 검증해야 합니다.

3. 뷰/인벤토리 네트워크 동기화 검증
- `UNetInventory`에 변경 이벤트는 추가했지만, 실제 UI 동기화 타이밍은 멀티플레이 상황에서 재검증이 필요합니다.

4. 자동 진행/제어 모드 확장 범위
- 로컬 쪽에서 나중에 자동 진행 토글을 추가하면 `_UNetPlay`에는 같은 규칙을 그대로 적용하지 않는 편이 안전할 수 있습니다.

## 권장 방향

1. `_UNetPlay`는 우선 로컬 규칙의 핵심 데이터 일관성만 따라간다
2. UI 편의 기능과 자동 진행은 로컬에서 먼저 안정화한다
3. 그 다음 멀티플레이에 필요한 범위만 선택적으로 반영한다
