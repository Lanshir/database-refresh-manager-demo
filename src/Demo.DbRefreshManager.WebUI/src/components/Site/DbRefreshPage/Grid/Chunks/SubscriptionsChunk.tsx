import { FC, useEffect } from 'react';
import { useSetAtom } from 'jotai';
import { useSubscription } from '@apollo/client';
import { changeJobPropsAction } from '@store/dbRefresh/dbRefreshActions';
import { GetOnDbRefreshJobChangeQuery, OnDbRefreshJobChangeSchema } from '@requests/graphql/subscriptions';

/**
 * Логика подписок на события web-socket. 
 */
const SubscriptionsChunk: FC = () =>
{
    const changeJobProps = useSetAtom(changeJobPropsAction);

    const jobChangeEvent = useSubscription<OnDbRefreshJobChangeSchema>(GetOnDbRefreshJobChangeQuery());

    // Запись задачи из события в список.
    useEffect(() => {
        const { loading, data } = jobChangeEvent;

        if (!loading && !!data) {
            changeJobProps(data.dbJobEvent.id!, data.dbJobEvent);
        }
    }, [changeJobProps, jobChangeEvent]);

    return null;
};

export default SubscriptionsChunk;