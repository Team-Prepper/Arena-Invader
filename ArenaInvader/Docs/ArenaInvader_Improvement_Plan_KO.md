# ArenaInvader 내부 개선 계획

작성일: 2026-06-23

이 문서는 지금 시점에서 `ArenaInvader` 내부에 남아 있는 개선 작업만 간단히 정리한 문서입니다.

## 1차 개선 완료

1. ✅ `Status` 규칙 일관성 정리
2. ✅ `MatchInfo` / 매치 설정 UI의 `IsAI` 편집 경로 정리
3. ✅ `GUIUnitPlayer` 상세 패널의 인벤토리 실시간 갱신 보강
4. ✅ `_UNetPlay`와 로컬 규칙 차이 정리
5. ✅ `MatchGenerator`와 `GameManager.MatchInfo`의 기본값 소스 정리
6. ✅ `Playground`의 플레이어 컨트롤러 생성/할당 경로 정리

## 다음 개선 후보

1. `GUIPlayerAction` / `OpenDiceUI` / `OpenInventory` 연결부의 턴 액션 흐름 정리
2. `BoardManager` / `GameMap`의 맵 상태 조회 책임 축소
3. `PlayableCharacter`와 `_UNetPlay` 컨트롤러 초기화 패턴 추가 정리

## 메모

- 자동 진행 토글 설계는 별도 문서로 분리했습니다.
  - [ArenaInvader_AutoPlay_Design_KO.md](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Docs/ArenaInvader_AutoPlay_Design_KO.md)
- `_UNetPlay` 차이는 별도 메모로 정리했습니다.
  - [UNetPlay_Difference_Notes_KO.md](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Docs/UNetPlay_Difference_Notes_KO.md)

- 지금 단계에서는 큰 구조 변경보다, 남은 규칙/연결부 보완을 우선합니다.
