# ArenaInvader 자동 진행 전환 설계 메모

작성일: 2026-06-23

이 문서는 `GUIPlayground`에 체크박스 UI를 추가해, 플레이 도중 `직접 조작`과 `AI 자동 진행`을 전환하는 기능을 나중에 구현하기 위한 설계 메모입니다.

## 목표

- `GUIPlayground`에 자동 진행 토글 UI를 둔다
- 토글 ON 시 현재 플레이어가 다음 행동부터 `AICharacterController`로 이어서 진행한다
- 토글 OFF 시 다시 `GUICharacterController`로 복귀한다
- 매치 시작 시점의 `IsAI`와, 플레이 도중의 자동 진행 상태를 구분한다

## 현재 구조 요약

현재 플레이어 조작 주체는 `ICharacterController` 1개로 결정됩니다.

- 사람 조작:
  - [GUICharacterController.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/CharacterController/GUICharacterController.cs)
- AI 조작:
  - [AICharacterController.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/CharacterController/AICharacterController.cs)
- 플레이어에 컨트롤러 주입:
  - [PlayableCharacter.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/Character/PlayableCharacter.cs)
  - [IPlayableCharacter.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/Character/IPlayableCharacter.cs)
- 매치 시작 시 초기 컨트롤러 할당:
  - [Playground.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/Playground.cs)

즉, 현재 구조는 이미 `컨트롤러 교체`만으로 사람/AI 전환이 가능한 형태입니다.

## 핵심 해석

`IsAI`는 "매치 시작 시 기본 조작자"로 보는 것이 적절합니다.

자동 진행 토글은 별도 런타임 상태로 분리하는 것이 좋습니다.

- `IsAI`
  - 매치 생성/설정 데이터
  - 시작 시 기본 컨트롤러 결정
- `AutoPlay`
  - 플레이 중 임시 전환 상태
  - UI 토글과 연결

## 권장 구조

### 1. 컨트롤러 참조 위치

`GUICharacterController`와 `AICharacterController`는 씬의 같은 게임 오브젝트에서 관리해도 됩니다.

추천 위치:

- `MatchGenerator`와 같은 게임 오브젝트
- 또는 `GUIPlayground`/`Playground` 전용 오브젝트

중요한 점은 "누가 들고 있느냐"보다 "현재 플레이어에 어떤 컨트롤러를 꽂을 수 있느냐"입니다.

### 2. 플레이어별 런타임 제어 상태

플레이어별로 아래 상태가 필요합니다.

- 현재 자동 진행 여부
- 현재 적용 중인 컨트롤러

가능한 구현 방식:

1. `PlayableCharacter`에 런타임 상태 추가
2. 별도 `PlayerControlMode` 컴포넌트 추가
3. `GUIPlayground`가 플레이어별 상태를 사전으로 보관

1차 구현은 3번 또는 1번이 가장 단순합니다.

## 1차 구현 권장 범위

처음에는 "현재 턴의 `PlayerAction` 단계에서만 전환 지원"이 가장 안전합니다.

이유:

- 이미 열려 있는 UI가 적다
- 흐름 재진입 지점이 단순하다
- 인벤토리/주사위/말 선택 중간 전환보다 버그 위험이 낮다

### 권장 동작

- 체크박스 ON
  - 현재 플레이어가 사람 조작 중이면 `AICharacterController`로 교체
  - `PlayerAction` 팝업을 닫고 AI 턴 시작 메서드 재호출
- 체크박스 OFF
  - 현재 플레이어를 `GUICharacterController`로 교체
  - 다음 행동부터 사람 조작

## 턴 중간 전환 시 고려사항

아래 상태는 즉시 전환 시 충돌 가능성이 있습니다.

- `GUIPlayerAction`가 열려 있는 상태
- `GUIInventory`가 열려 있는 상태
- `GUIDice`가 열려 있는 상태
- `GUISelectMovePawn`가 열려 있는 상태
- `GUIShop`이 열려 있는 상태

따라서 전환 규칙이 필요합니다.

### 안전한 규칙 A

- `PlayerAction` 상태에서만 즉시 전환
- 그 외 상태에서는 "다음 턴부터 적용"

### 안전한 규칙 B

- 전환 시 현재 조작 팝업을 닫는다
- 현재 단계에 맞는 진입 메서드를 새 컨트롤러로 다시 호출한다

1차 구현은 A가 더 안전합니다.

## 필요한 코드 변경 후보

### UI

- [GUIPlayground.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/GUIPlayground.cs)
  - 자동 진행 체크박스 참조 추가
  - 현재 플레이어 기준 토글 이벤트 처리

### 제어 상태

- [PlayableCharacter.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/Character/PlayableCharacter.cs)
  - 현재 컨트롤러 보관 방식 명확화
  - 필요 시 `GetController()` 또는 `SetControlMode()` 성격 메서드 추가

또는 별도 파일 추가:

- `PlayerControlMode.cs`
- `PlayableCharacterControlState.cs`

### 컨트롤러 공급

- [Playground.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/Playground.cs)
  - 시작 시 컨트롤러 할당만 담당
  - 런타임 전환 책임은 넘기는 것이 좋음

- [MatchGenerator.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/MatchGenerator/MatchGenerator.cs)
  - 같은 오브젝트에 있는 `AICharacterController` 참조를 제공하는 용도로는 사용 가능
  - 하지만 토글 로직 자체를 넣는 것은 권장하지 않음

### AI 재진입

- [AICharacterController.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/CharacterController/AICharacterController.cs)
  - 현재는 `StartTurn()`, `Inventory()`, `RollDice()`, `Shop()` 진입점이 존재
  - 자동 진행 ON 시 어느 진입점을 호출할지 단계별 규칙 정리가 필요

## 추천 구현 순서

1. `GUIPlayground`에 체크박스 UI 추가
2. 현재 플레이어 자동 진행 상태를 저장할 최소 구조 추가
3. `PlayerAction` 단계에서만 즉시 전환 지원
4. 사람/AI 컨트롤러 참조를 씬 오브젝트 기반으로 정리
5. 필요 시 `Inventory`, `Dice`, `SelectMovePawn` 단계 전환 확장

## 장점

- 플레이 도중 자동 진행 ON/OFF 가능
- `IsAI`와 런타임 자동 진행 상태를 분리할 수 있음
- 자동사냥, 데모 플레이, 관전용 AI 확장 기반이 됨

## 주의점

- `SetController()`만 바꾸고 열린 UI를 그대로 두면 조작 주체가 섞일 수 있음
- 따라서 "전환 가능한 단계"를 제한하거나 "전환 시 UI 정리" 규칙이 반드시 필요함
- 로컬 플레이와 `_UNetPlay`의 적용 범위를 처음부터 분리해서 생각하는 편이 안전함

## 결론

현재 구조에서도 자동 진행 토글은 충분히 구현 가능합니다.  
다만 핵심은 `AICharacterController`를 어디에 둘지보다, "지금 턴의 어느 단계에서 새 컨트롤러가 이어받을지"를 명확히 정하는 것입니다.

가장 안전한 1차 구현은 다음과 같습니다.

1. `GUIPlayground` 체크박스 추가
2. 플레이어별 자동 진행 상태 저장
3. `PlayerAction` 단계에서만 즉시 AI 전환 허용
4. 나머지 단계는 다음 턴부터 적용
