
# FSM 상태 기반으로 특정 UI의 표시 여부 결정
현재 화면에 어떤 UI를 표시할 것인가를 FSM 상태기반으로 결정한다.  
사용자의 의도(사용자의 입력/현재 앱 상태에 대한 인식)를 반영하는 UserStory FSM을 설계하고 이를 FSM Tool을 사용해 정의한다. 
사용자에 의해 발생하는 상태전이 입력은 모두 UserStory 레이어로 입력되도록 한다.  
특정 FSM에 상태가 밀집되지 않도록 사용자의 인식상태를 고려하여 여러 개의 FSM으로 나눠서 설계하고 동일한 레이어 내에서 상태에 따라 전환시켜 사용한다.   
사용자의 의도나 인식과 무관한 별도의 상태관리가 필요할 경우 레이어를 추가해서 사용한다. 

## Reaction 사용
GameObject에  ReactionByState 스크립트를 Attach 시키면 특정 상태에 대한 Reaction을 지정할 수 있다. Canvas나 Panel 등에 주로 사용한다. 

#### ReactionByState 참고사항
상태 등록 초기화 과정을 위해서 반드시 실행시 Active 상태여야 한다.  
ReactionByState가 붙어있는 오브젝트를 포함한 하위 오브젝트는 Awake 이 후 자동으로 Deactive상태로 전환되기 때문에 OnEnable/Start는 Reaction에 의해서 재활성화 되기전까지 호출 되지 않는다. 해당 함수를 사용하는 경우 초기화 시점을 주의하여야 한다. 

Reaction_Expand를 상속받아서 확장하여 사용가능하다. 

## 레이어 내의 FSM 전환 방식
- USMain_[State] 상태 시작시 Layer.ChangeFSM([State]) 실행  
- 각 FSM의 End State 이벤트에 OnStart_US_EndState / OnEnd_US_EndState 함수 등록  
- 각 UserStory FSM의 End State.EventStart 발생시 ChangeFSM() -> SetEscape -> OutroState 루틴이 실행된 후 US_MainMenu로 진입  
- 각 UserSotry FSM의 End State.EventResume 발생시 HistoryBack을 통해 마지막 상태로 재진입 한다.   


# 데이터와 UI의 연결은 Bindable과 UIBind를 사용한다.

## Binadble / BindRepo
Bindable 클래스는 프로퍼티를 이용해서 바인딩된 데이터의 변화가 있을 때마다 OnChangedData이벤트를 발생시킨다. 내부로직에서 UI로 노출되야 하는 데이터의 경우 Bindable 타입으로 생성하고 미리 정의된 Bind_Idx를 키값으로 BindRepo에 등록한다. BindRepo는 Bindable 데이터를 전역으로 사용할 수 있게 한다. 

## UIBind
Bind_Idx를 인스펙터에서 지정하여 런타임에서 자동으로 Bindable 데이터와 UI를 연결한다. 
Bindable 매개변수 타입과 사용하려는 UI의 종류에 따라 확장해서 사용한다. 


