import { FC } from 'react';
import { FlexCol } from '@shared/components';
import dayjs from 'dayjs';

interface ChunkProps {
    manualRefreshDate?: string | null,
    scheduleRefreshDate: string,
    lastRefreshDate: string,
    scheduleIsActive: boolean
}

/** Текст ячейки расписания перезаливок. */
const RefreshScheduleChunk: FC<ChunkProps> = (
    { manualRefreshDate, scheduleRefreshDate, lastRefreshDate, scheduleIsActive }
) => {
    const dateManual = dayjs(manualRefreshDate);
    const dateScheduled = dayjs(scheduleRefreshDate);
    const dateLast = dayjs(lastRefreshDate);

    const dateNext = dateManual.isValid()
        && (!scheduleIsActive || dateManual.isBefore(dateScheduled))
        ? dateManual : dateScheduled;

    const dateNextString = scheduleIsActive || !!manualRefreshDate
        ? dateNext.format('dd DD.MM.YY HH:mm')
        : ' - - - - - - - - - - - - - - -';

    // Подсветка строки где перезаливка была более чем 3 дня назад.
    const refreshWarnColor = dateLast.diff(dayjs(), 'day') > -3
        ? 'inherit' : 'red';

    return (
        <FlexCol>
            <span>{`Next: ${dateNextString}`}</span>

            <span>{`Last: ${dateLast.format('dd DD.MM.YY HH:mm')}`}</span>

            <span style={{ color: refreshWarnColor }}>
                {`Last +3д.: ${dateLast.add(3, 'days').format('dd DD.MM.YY HH:mm')}`}
            </span>
        </FlexCol>
    );
};

export default RefreshScheduleChunk;