# Vworld에서 3D 텍스쳐 건물들의 정보를 가져오는 소스입니다.

## 개요
-------------
 건물을 클릭하게 되면 아래와 같이 건물의 정보가 나타납니다. 웹 디버깅 툴인 피들러를 통해 HTTP 요청시 GET 방식으로 UFID를 넘겨주어 정보들은 응답받는 것을 확인하였습니다. 해당 소스는 20개의 건물 정보를 가져와서 조건에 따라 해당되는 건물들을 리스트에 보여주는 소스입니다. 
 
 ![main](./screenshot/BuildingInfo.PNG)

## 사용법
-------------
 코드를 복붙!!.
### 기능
- 실행 시 메인 창
![main](./screenshot/main.PNG)
 1. 'Key' 를 적는 칸에 UFID를 적어주시고, [Bring it] 버튼을 누르게 되면 해당 UFID를 가진 건물의 정보가 리스트에 추가됩니다.
 2. 오른쪽 상단에 'Attr' 라는 콤보박스에서 찾으실 속성 값을 선택하시고, 옆 빈칸에 찾으실 값을 입력한 다음 Search 버튼을 누르시면 됩니다.
	- 찾을 값이 숫자일 경우 써준 숫자보다 미만인 값들을 찾아냅니다.
	- 찾을 값이 문자일 경우 써준 문자가 포함된 값들을 찾아냅니다.
 3. 'Restore' 버튼은 초기의 값들로 원상복귀 시켜줍니다.

### 주요 함수
#### htmlRequest(string ufid)
```
htmlRequest([UFID])   // UFID를 넘겨주어 HTML Body를 response로 받아 옵니다.
```
#### HtmlParseAction(string html)
```
htmlRequest([HTML])   // HTML Body를 받아 parsing 하여 사용할 정보를 얻고, 리스트에 추가합니다.
```

### 결과 
 수정 중






