export interface Stats {
    totalUsers: number;
    avgFriendsCount: number;
}

export interface Relation {
    friendsSetId: number;
    firstPersonId: number;
    secondPersonId: number;
}