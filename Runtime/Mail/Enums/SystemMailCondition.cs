namespace Glitch9.Game.MailSystem
{
    public enum SystemMailCondition : short
    {
        None = 0,
        /// <summary>
        /// 플레이어 레벨에 따른 조건 확인
        /// </summary>
        /// <remarks>
        /// sample : 10 (10레벨 이상시)
        /// </remarks>
        PlayerLevel,
        /// <summary>
        /// 로그인 횟수에 따른 조건 확인
        /// </summary>
        /// <remarks>
        /// sample : 3 (3일 이상 로그인시)
        /// </remarks>
        LoginCount,
        /// <summary>
        /// 구매 횟수에 따른 조건 확인
        /// </summary>
        /// <remarks>
        /// sample : 3 (3번 이상 구매시)
        /// </remarks>
        PurchaseCount,
        /// <summary>
        /// 이벤트 참여 여부에 따른 조건 확인
        /// </summary>
        /// <remarks>
        /// sample : 2023-03-01~2023-12-01 (2023년 3월 1일부터 12월 1일까지 참여시)
        /// </remarks>
        SeasonalEvent,
        /// <summary>
        /// 업적 완료 여부에 따른 조건 확인
        /// </summary>
        /// <remarks>
        /// sample : Total_RoutineCompletion (업적 ApiEnumDE / MissionObjective)
        /// </remarks>
        AchievementCompletion,
        /// <summary>
        /// 친구 추천 수에 따른 조건 확인
        /// </summary>
        /// <remarks>
        /// sample : 3 (3명 이상 추천시)
        /// </remarks>
        FriendReferralCount,
        /// <summary>
        /// 특정 날짜 또는 기념일 조건 확인 (예: 생일, 게임 출시 기념일)
        /// </summary>
        /// <remarks>
        /// sample : 2023-03-01 (2023년 3월 1일)
        /// </remarks>
        SpecialDate
    }
}