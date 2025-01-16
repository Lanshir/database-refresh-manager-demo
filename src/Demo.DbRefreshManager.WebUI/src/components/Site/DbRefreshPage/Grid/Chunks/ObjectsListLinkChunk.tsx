import { FC } from 'react';
import { useAtomValue } from 'jotai';
import { configState } from '@store/config/configState';
import { IconButton, Link } from '@mui/material';
import { List } from '@mui/icons-material';

interface ChunkProps {
    dbName: string
}

/**
 * Ссылка на список объектов. 
 */
const ObjectsListLinkChunk: FC<ChunkProps> = ({ dbName }) =>
{
    const config = useAtomValue(configState);

    return (
        <IconButton size="large" LinkComponent={Link} target="_blank"
            href={`${config.objectsListUrl}${dbName}`}
        >
            <List />
        </IconButton>
    );
};

export default ObjectsListLinkChunk;