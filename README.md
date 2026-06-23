# Arena Invader

`Arena Invader`는 윷놀이식 경로 이동과 AOS 전투를 결합한 Unity 기반 턴제 보드 게임 프로젝트입니다.  
플레이어는 주사위/다트/룰렛으로 이동 값을 얻고, 보드 위 기물을 움직이며 스탯을 성장시키고 전투를 통해 최후의 1인을 가립니다.

## 프로젝트 요약

- 장르: 턴제 보드 전략 + 전투
- 엔진: Unity 6
- 현재 프로젝트 에디터 버전: `6000.3.11f1`
- 지원 형태:
  - 로컬 플레이
  - `Unity Netcode for GameObjects` 기반 멀티플레이
  - AI 플레이어 제어
- 기본 시작 씬: `ArenaInvader/Assets/Scenes/GeneratorSelect.unity`

## 핵심 게임플레이

- 경로 기반 보드판 이동
- 기물 겹침 및 업기 처리
- 도착 타일 이벤트
  - 공격력 증가
  - 방어력 증가
  - 체력 증가
  - 전투 시작
- 오브젝트 적(`Baron`)과 플레이어 간 전투
- 아이템 인벤토리 및 소비형 효과
- 턴제 진행 시스템
- 플레이어별 수동 조작 / AI 조작 전환
- 다국어 문자열 리소스 기반 언어 변경

## 기술 스택 및 의존성

- Unity `6000.3.11f1`
- `com.unity.netcode.gameobjects` `2.9.1`
- `com.unity.nuget.newtonsoft-json` `3.2.2`
- `com.unity.ugui` `2.0.0`
- `com.unity.shadergraph` `17.3.0`

프로젝트 내부에는 다음과 같은 자체/로컬 모듈이 함께 사용됩니다.

- `EasyH`: UI, 리소스 접근, 언어 시스템, 싱글턴 유틸리티
- `EasyH.Gaming.TurnBased`: 턴 진행 시스템
- `BKTools`: 주사위 관련 로직

## 실행 방법

### Unity에서 실행

1. Unity Hub에서 `ArenaInvader` 폴더를 엽니다.
2. 에디터 버전은 가능하면 `6000.3.11f1`을 사용합니다.
3. `ArenaInvader/Assets/Scenes/GeneratorSelect.unity` 씬을 엽니다.
4. Play를 실행합니다.
5. 매치 설정 UI에서 플레이어 수, 캐릭터, AI 여부, 맵, 주사위 방식을 정한 뒤 게임을 시작합니다.

### WebGL 빌드 확인

저장소에는 `build/WebGL` 결과물이 포함되어 있습니다.

- 진입 파일: `build/WebGL/index.html`
- 로컬 확인 시에는 브라우저에서 직접 여는 대신 정적 서버로 서빙하는 편이 안전합니다.

예시:

```bash
cd build/WebGL
python3 -m http.server 8000
```

그 뒤 브라우저에서 `http://localhost:8000`으로 접속하면 됩니다.

## 씬 및 흐름

### 주요 씬

- `GeneratorSelect.unity`: 현재 빌드 설정에 포함된 기본 시작 씬
- `Title.unity`: 저장소에는 존재하지만 현재 빌드 설정에서는 비활성화
- `BK_DICE.unity`, `EasyH.unity`, `SampleScene.unity`, `art.unity`: 테스트/보조 성격의 씬으로 보임

### 로컬 플레이 흐름

`GameManager`가 기본 `Playground`를 생성하고, `MatchGenerator`가 매치 정보에 따라 플레이어와 맵을 구성합니다.  
게임 시작 후 `GUIPlayground`가 열리고, 턴 시스템이 각 플레이어의 행동을 진행합니다.

### 멀티플레이 흐름

- `UNetNetwork`가 `NetworkManager` 프리팹을 로드해 Host/Client를 시작합니다.
- Host는 `UNetPlayground`를 스폰합니다.
- `UNetMatchInfo`가 네트워크 플레이어 정보와 로컬 UI 상태를 동기화합니다.
- 모든 플레이어 준비가 끝나면 턴 시스템이 시작됩니다.

## AI 동작 개요

`AICharacterController`가 다음 순서로 행동합니다.

1. 인벤토리에 아이템이 있으면 우선 사용
2. 없으면 주사위 UI를 열어 자동 굴림
3. 이동 가능한 기물 중 기대 가치가 가장 높은 대상을 선택
4. 상점에서는 가성비가 높은 아이템을 구매

즉, 현재 AI는 규칙 기반 자동 플레이에 가깝고, 외부 API 기반 AI는 아직 구현 단계로 보입니다.

## 디렉터리 구조

```text
.
├── README.md
├── build/WebGL/                 # WebGL 빌드 산출물
└── ArenaInvader/
    ├── Assets/
    │   ├── Scenes/              # Unity 씬
    │   ├── Scripts/
    │   │   ├── ArenaInvader/    # 로컬 게임 로직
    │   │   └── _UNetPlay/       # Netcode 기반 멀티플레이 로직
    │   ├── Resources/           # 맵, 데이터, 프리팹, 문자열 리소스
    │   ├── Sprites/
    │   └── SFX/
    ├── Packages/
    └── ProjectSettings/
```

## 현재 확인된 상태

- README에 적혀 있던 Unity 버전(`6000.0.58f2`)과 실제 프로젝트 버전이 달랐습니다.
  - 실제 `ProjectSettings/ProjectVersion.txt` 기준 버전은 `6000.3.11f1`입니다.
- 빌드 설정에는 현재 `GeneratorSelect.unity`만 활성화되어 있습니다.
- 플레이어별 AI 여부를 런타임에 반영하는 로직이 로컬 `Playground`에 포함되어 있습니다.
- 멀티플레이 쪽은 `Unity Netcode for GameObjects` 기준으로 구성되어 있으며, 과거 `UNet` 네이밍이 일부 클래스명에 남아 있습니다.

## 문서

추가 문서는 `ArenaInvader/Docs` 아래에 정리되어 있습니다.

- `Project_Assessment_KO.md`
- `ArenaInvader_Improvement_Plan_KO.md`
- `ArenaInvader_AutoPlay_Design_KO.md`
- `ArenaInvader_Game_System_Design_Principles_KO.md`
- `ArenaInvader_Visual_Design_Principles_KO.md`
- `UNetPlay_Test_Checklist_KO.md`
- `UNetPlay_Difference_Notes_KO.md`
- `UNetPlay_TODO_KO.md`

## 알려진 주의사항

- 저장소에 매우 많은 에셋과 WebGL 빌드 결과물이 포함되어 있어 clone/import 시간이 길 수 있습니다.
- 일부 클래스명은 `UNet` 접두사를 사용하지만 실제 패키지는 `Netcode for GameObjects`입니다.
- 테스트 자동화보다는 Unity 에디터 기반 수동 검증 흐름에 가까운 구조입니다.

## 라이선스

이 프로젝트는 [MIT License](LICENSE)를 따릅니다.

## 참고 링크

- 세부 기획: https://easy-h.notion.site/2024-1355e129e5ff8050afa4e1b23c7b184e?pvs=74
- 참고 자료: https://gofogo.tistory.com/64

## 연락처

- `skysea001010@gmail.com`
