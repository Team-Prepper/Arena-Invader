# Arena Invader 프로젝트 평가

작성일: 2026-06-22

이 문서는 현재 프로젝트의 구조와 코드를 빠르게 훑어본 뒤, 앞으로 보완하면 좋은 지점을 우선순위 중심으로 정리한 문서입니다.  
목표는 기능 설명이 아니라, 유지보수성·안정성·확장성 관점에서 “어디를 먼저 손보면 효과가 큰지”를 잡아내는 것입니다.

## 한줄 평가

현재 프로젝트는 게임 플레이 기능은 꽤 많이 쌓여 있지만, 공용 유틸과 게임 로직, UI 연동이 서로 강하게 얽혀 있어 변경 비용이 커질 가능성이 높습니다.  
특히 전역 싱글턴, `Resources` 기반 로딩, 암묵적 의존성, 그리고 일부 유틸 함수의 안전성 문제를 먼저 정리하면 전체 품질이 크게 좋아질 가능성이 큽니다.

## 현재 보이는 강점

1. `EasyH`와 `ArenaInvader`로 역할을 나누려는 흔적이 있습니다.
2. 턴 기반, 경로 기반, 인벤토리, UI, 사운드 같은 시스템을 별도 네임스페이스/폴더로 분리해 두었습니다.
3. 인터페이스를 많이 사용해서, 의도상으로는 교체 가능한 구조를 지향하고 있습니다.
4. 리소스 데이터가 JSON/XML/ScriptableObject 등 여러 형태로 정리되어 있어 콘텐츠 확장 가능성은 있습니다.

## 우선 보완해야 할 부분

### 1. 전역 싱글턴과 암묵적 의존성이 너무 많음

- 관련 파일:
  - [Singleton.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/EasyH/Core/Singleton.cs)
  - [MonoSingleton.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/EasyH/Unity/MonoSingleton.cs)
  - [UIManager.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/EasyH/Unity/UIKit/UIManager.cs)
  - [BoardManager.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/Board/BoardManager.cs)

문제점:

- `Instance` 접근이 어디서나 가능해서, 코드만 봐서는 데이터 흐름을 추적하기 어렵습니다.
- 초기화 순서에 따라 런타임 오류가 나기 쉬운 구조입니다.
- 테스트나 시뮬레이션 환경에서 대체 구현을 넣기 어렵습니다.

권장 개선:

- 게임 핵심 서비스는 가능한 한 명시적 참조로 바꾸기.
- 싱글턴은 정말 필요한 진입점만 남기기.
- `Instance` 내부에서 생성하는 방식보다, 씬 부트스트랩이나 DI 스타일의 생성 경로를 따로 두기.

우선순위:

- 매우 높음

### 2. `Resources` 기반 로딩에 의존하는 UI 구조

- 관련 파일:
  - [UIManager.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/EasyH/Unity/UIKit/UIManager.cs)
  - `Assets/Resources/...`

문제점:

- 문자열 키와 리소스 경로가 분리되어 있어 오타가 나면 런타임에서만 발견됩니다.
- `Resources`는 편하지만, 프로젝트가 커질수록 의존성과 배포 제어가 약해집니다.
- UI 프리팹과 키 매핑이 외부 JSON에만 의존하면, 리팩터링 시 추적이 어렵습니다.

권장 개선:

- UI 등록부를 ScriptableObject 또는 중앙 레지스트리로 옮기기.
- 최소한 키/경로 검증용 에디터 툴을 추가하기.
- `OpenGUI<T>` 호출부에서 타입과 키의 일관성을 검증하기.

우선순위:

- 높음

### 3. 공용 유틸 함수의 안전성 문제

- 관련 파일:
  - [Useful.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/EasyH/UtilKit/Useful.cs)

문제점:

- `Combination<T>`가 입력 배열을 직접 변경합니다.
- `Random.Range(0, i)`는 마지막 원소를 포함하지 못하는 형태라, 의도한 셔플/추출이 아닐 가능성이 큽니다.
- `count > array.Length`에 대한 방어가 없습니다.
- 결과적으로 호출부에서 “순수 함수처럼 보이지만 사실은 부작용이 있는 함수”가 됩니다.

권장 개선:

- 입력 배열을 복사해서 처리하기.
- 경계값 검증 추가하기.
- 함수 이름도 실제 동작에 맞게 `PickRandomSubset`, `ShuffleAndTake`처럼 더 명확하게 바꾸기.

우선순위:

- 매우 높음

### 4. ✅ `PlayableCharacter`의 책임이 너무 큼

- 관련 파일:
  - [PlayableCharacter.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/Character/PlayableCharacter.cs)

문제점:

- 초기화, 상태 구독, 턴 진행, UI 열기, 사운드 재생, 컨트롤러 위임, 아이템 사용 등 책임이 한 클래스에 집중되어 있습니다.
- `Awake()`에서 여러 인터페이스를 무조건 가져오므로, 구성 요소가 하나만 빠져도 런타임 에러가 납니다.
- `_selector`가 없을 때와 있을 때의 흐름이 섞여 있어, 턴/행동 흐름이 읽기 어렵습니다.
- `_chance`와 `_extraDicePoint`는 상태 전이가 명확한 도메인 객체로 분리할 수 있습니다.

권장 개선:

- 캐릭터의 상태 머신과 UI 오픈, 턴 진행을 분리하기.
- `RequireComponent` 또는 초기화 검증 코드를 추가하기.
- `StartTurn`, `OpenRollDice`, `EndTurn` 흐름을 작은 서비스로 나누기.

우선순위:

- 매우 높음

### 5. ✅ `BoardManager`의 판정 로직이 단순 자료구조에 과의존

- 관련 파일:
  - [BoardManager.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/Board/BoardManager.cs)

문제점:

- `Plate`를 키로 쓰는 맵과 이벤트 맵이 직접 관리되는데, 생명주기 관리가 보이지 않습니다.
- `ResetPawnAt`, `SetPawnAt`, `OverlapProcess` 사이의 상태 일관성을 호출부가 책임져야 합니다.
- 같은 팀/다른 팀 처리 전략이 고정 객체로 박혀 있어 확장성이 제한됩니다.

권장 개선:

- 판 상태를 `BoardState` 같은 별도 객체로 분리하기.
- pawn 위치 변경과 overlap 처리를 하나의 명시적 API로 합치기.
- 팀별 overlap 정책을 전략 패턴으로 분리하기.

우선순위:

- 높음

## 중간 우선순위 개선 과제

### 1. 네임스페이스와 디렉터리 구조를 더 엄격히 정리

현재는 공용 프레임워크 성격의 `EasyH`와 게임 전용 `ArenaInvader`가 공존합니다.  
이 방향은 좋지만, 일부 클래스는 네임스페이스가 없거나 위치와 역할이 완전히 일치하지 않아 보입니다.

권장:

- 공용 코드와 게임 코드의 의존 방향을 한쪽으로만 흐르게 만들기.
- 에디터 전용 코드와 런타임 코드를 더 분명히 분리하기.
- 네임스페이스 없이 남은 클래스들을 정리하기.

### 2. 초기화 실패 시점을 더 빠르게 드러내기

예를 들면 `PlayableCharacter.Awake()`는 여러 컴포넌트를 한 번에 가져오는데, 하나만 빠져도 원인을 찾기 어렵습니다.

권장:

- `TryGetComponent` 또는 명시적 검증 메서드 추가.
- 필수 의존성 누락 시 친절한 에러 메시지 출력.
- 씬 로딩 후 자동 검증 루틴 추가.

### 3. 문자열 키와 리소스 데이터 검증

현재는 `GUIInfor`, `MessageBox`, `TurnStart`처럼 문자열 키에 많이 의존합니다.

권장:

- 키를 상수화하거나 enum화하기.
- 에디터에서 키 누락을 검사하는 도구 만들기.
- 런타임에서 fallback 동작을 명시하기.

### 4. 디버그 로그 정리

실험용 로그가 남아 있는 부분이 있습니다. 예를 들어 `PlayableCharacter.SetController()`의 `Debug.Log` 같은 것은 디버깅이 끝나면 제거하거나 조건부로 바꾸는 편이 좋습니다.

권장:

- `#if UNITY_EDITOR` 또는 로그 래퍼 도입.
- 실제 운영 로그와 개발 로그를 분리하기.

## 장기적으로 좋아지는 방향

1. 핵심 게임 흐름을 상태 머신으로 정리하기.
2. UI와 게임 규칙의 결합도를 낮추기.
3. 데이터 로딩 레이어를 단일 인터페이스로 통일하기.
4. 에디터 검증 도구를 만들어 문자열 키와 리소스 누락을 사전에 잡기.
5. 최소한의 자동 테스트라도 도입해서, 셔플/이동/판정 같은 순수 로직부터 보호하기.

## 바로 손보면 좋은 순서

1. `UIManager`의 리소스 매핑 검증 강화
2. 전역 싱글턴 의존 축소
3. `Useful.Combination` 안전성 수정
4. `PlayableCharacter` 추가 분리 작업
5. `BoardManager` 상태 분리 보강

## EasyH 공용 모듈 개선 후보

아래 항목들은 현재 구조와 public API를 유지한 채로 내부만 개선할 수 있어서, 다른 프로젝트에 대한 영향이 비교적 적습니다.

### 1. ✅ `Useful.Combination` 내부 안전성 보강

- 관련 파일:
  - [Useful.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/EasyH/UtilKit/Useful.cs)

개선 방향:

- 입력 배열을 직접 수정하지 않도록 내부 복사본 사용
- `count`, `array` 길이 검증 추가
- 랜덤 범위 오류 수정

영향도:

- public 메서드 시그니처를 유지하면 외부 프로젝트 호환성에 거의 영향이 없습니다.

### 2. ⬜ `UIManager` 내부 검증 강화

- 관련 파일:
  - [UIManager.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/EasyH/Unity/UIKit/UIManager.cs)

개선 방향:

- `OpenGUI<T>`에서 키 누락 시 더 명확한 예외 또는 경고 제공
- `NowDisplay`, `uiStack`, `_dic` 초기화 상태 검증
- `SceneManager.sceneLoaded` 재등록 방지

영향도:

- 메서드 이름과 사용 방식은 그대로 두고, 내부 예외 처리만 보강하면 됩니다.

### 3. ✅ `ResourceManager` 초기화 안정화

- 관련 파일:
  - [ResourceManager.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/EasyH/Unity/Core/ResourceManager.cs)

개선 방향:

- `ResourcesResourceConnector` 중복 부착 방지
- `ResourceConnector` null 검증
- 이후 커넥터 교체가 가능하도록 내부 생성 경로 정리

영향도:

- 외부에서는 여전히 `ResourceManager.Instance.ResourceConnector`만 보면 되므로 호환성 위험이 낮습니다.

### 4. ✅ `Singleton<T>` 및 `MonoSingleton<T>`의 내부 방어 코드 강화

- 관련 파일:
  - [Singleton.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/EasyH/Core/Singleton.cs)
  - [MonoSingleton.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/EasyH/Unity/MonoSingleton.cs)

개선 방향:

- 중복 초기화 방지
- 생성 실패 시점에 더 명확한 메시지 제공
- 에디터/플레이 모드에서의 동작 차이 정리

영향도:

- public 접근 방식은 유지하고 내부 안정성만 높이면 됩니다.

## EasyH 우선순위 해석

공용 모듈은 구조를 바꾸기보다 내부 안전성을 올리는 편이 더 적절합니다.  
따라서 실제 작업 순서는 다음처럼 보는 게 좋습니다.

1. `Useful.Combination` 안전성 수정
2. `UIManager` 내부 검증 강화
3. `ResourceManager` 초기화 안정화
4. `Singleton<T>` / `MonoSingleton<T>` 방어 코드 정리

## 완료 표시

- ✅ `PlayableCharacter` 초기화 검증 추가
- ✅ `BoardManager` 방어적 조회/등록 개선
- ✅ `Useful.Combination` 안전성 수정
- ✅ `UIManager`의 리소스 매핑 검증 강화
- ✅ `ResourceManager` 초기화 안정화
- ✅ `Singleton<T>` / `MonoSingleton<T>` 방어 코드 정리
- ⬜ 전역 싱글턴 의존 축소

## 참고

검토 기준은 다음 파일들을 중심으로 잡았습니다.

- [Useful.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/EasyH/UtilKit/Useful.cs)
- [UIManager.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/EasyH/Unity/UIKit/UIManager.cs)
- [PlayableCharacter.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/Character/PlayableCharacter.cs)
- [BoardManager.cs](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/Assets/Scripts/ArenaInvader/Board/BoardManager.cs)
- [ProjectVersion.txt](/Users/easyh/Documents/GitHub/Easy-H/Arena-Invader/ArenaInvader/ProjectSettings/ProjectVersion.txt)
