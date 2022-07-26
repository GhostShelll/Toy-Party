# Lotto-Generator
Lotto 번호 생성기
회차 별 당첨 번호가 나온 횟수를 종합하여 번호를 선택할 수 있습니다.

## Unity Version
2021.3.4f1

## Version Info
### 1.0.2
- **번호가 '많이 나온 횟수'와 '적게 나온 횟수'의 평균 값을 기준으로 번호를 선택할 수 있게하는 Toggle 기능 구현**
- Unity Package Version 최신화
- **Table 외부에서 읽어오기 기능 구현**
  - [Google Sheets To Unity](https://assetstore.unity.com/packages/tools/utilities/google-sheets-to-unity-73410) 외부 Asset 사용
  - Built-In Table 제거 (LottoResultData, LottoOriginResultData)
### 1.0.0
- Unity Project Setting
- Scene 구조 정립
- Popup 구조 정립
- Built-In 된 Table 정보를 읽어오는 Asset Manager 구현
  - LocaleData : 한국어, 영어 관련 Table
  - LottoResultData : Lotto 회차 별 결과 Table
  - LottoOriginResultData : [로또6/45](https://www.dhlottery.co.kr/gameResult.do?method=byWin)에서 제공하는 회차 별 결과 Table, LottoResultData Table의 검사용
- 효과음 담당 Manager 구현
- **6자리의 Lotto 번호를 선택할 수 있는 Popup 구현**
  - 많이 나온 번호 순으로 번호 선택
  - 적게 나온 번호 순으로 번호 선택
  - 선택된 모든 번호 List를 토대로 Random 지정하여 6개의 번호 추출
- Bug 수정