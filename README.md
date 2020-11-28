# yutnori

*Unity 2019.4.9f1 버전에서 개발 하였으며, 다른 버전에서는 호환되지 않을 수 있습니다.*

Unity설치 후 프로젝트 열고 Build and Run으로 실행 가능하나, Unity 설치가 어려울땐 "Yutnori.exe"로 실행 가능.

2인 플레이가 가능하며, 테스트를 위해 1인플레이도 게임 진행 가능하게 설계 했습니다.

* 최초 방에 입장한 사람이 방장이 되고, GameStart버튼은 방장만이 누를수 있습니다.

* 원활한 게임 테스트를 위해 master key를 구현하였으며, q, w, e, r, t, y키를 이용해
도, 개, 걸, 윷, 모, 뒷도가 나오게 할 수 있습니다.


Photon2 SDK를 이용해 서버를 구축하였으며, 플레이어의 말 이미지는 Asset Store에 있는 Free Asset을 사용했습니다.

직접 작성한 코드는 "Yutnori\Assets\Scripts"에 있고, 모든 Prefabs들은 "Yutnori\Assets\Resources"에 있습니다.