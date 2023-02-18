import React from "react";
import {useNavigation, useParams} from "react-router-dom";
import {useGetUserById} from "@/features/user/api/getUserById";
import {MainLayout} from "@/components/Layout/MainLayout";
import {Head} from "@/components/Head";
import {ContentLayout} from "@/components/Layout/ContentLayout";
import {UserProfile} from "@/features/user/routes/UserProfilePage";

export const OtherUserPage: React.FC = () => {
    const userId = Number(useParams<{id: string}>().id);
    const {data: user, isLoading} = useGetUserById({userId, config: {}});
    
    return (    
    <MainLayout>
        <Head title={`Просомтр профиля ${user?.name ?? ""} ${user?.surname ?? ""}`}/>
        <ContentLayout>
            {user && (<UserProfile user={user} isMe={false}/>)}
        </ContentLayout>
    </MainLayout>);
}