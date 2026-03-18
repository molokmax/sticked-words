import { UIEvent } from 'react';
import { useEffect, useState } from 'react';
import { Link } from 'react-router';
import FlashCardListItem from '../FlashCardListItem';
import Loading from '../Loading';
import { PageQuery } from '../../models/PageQuery';
import { ErrorHandler } from '../../services/ErrorHandler';
import { FlashCardService } from '../../services/FlashCardService';
import { FlashCardShort } from '../../models/FlashCardShort';

import './FlashCardList.scss'

function FlashCardList() {

    const [loading, setLoading] = useState(false);
    const [total, setTotal] = useState(0);
    const [data, setData] = useState<FlashCardShort[]>([]);
    const [error, setError] = useState<string | null>(null);

    const service = new FlashCardService();

    const loadData = async (reload = false) => {
        try {
            setLoading(true);
            const skip = reload ? 0 : data.length;
            const query = new PageQuery(reload, skip);
            const page = await service.getByQuery(query);
            const newData = reload ? page.data : [...data, ...page.data];
            setData(newData);
            if (page.total != null) { // null or undefined
                setTotal(page.total);
            }
        } catch (err) {
            setData([]);
            setTotal(0);
            setError(ErrorHandler.getMessage(err));
        } finally {
            setLoading(false);
        }
    }

    async function onScrolled(ev: UIEvent<HTMLDivElement>) {
        const container = ev.currentTarget;
        const threshold = 100;
        const toBottom = container.scrollHeight - (container.scrollTop + container.clientHeight);
        if (toBottom < threshold && !loading && data.length < total) {
            await loadData();
        }
    };

    useEffect(() => {
        loadData(true);
    }, []);

    return (
        <div className="flash-card-list">
            <div className="flash-card-list__header">
                <Link to="add" className="flash-card-list__add-word">Add</Link>
                <Link to="session" className="flash-card-list__start-session">Learn</Link>
                <div className="flash-card-list__words-count">Words: { total }</div>
            </div>
            {
                loading
                    ? <Loading />
                    : (
                        <div className="scroll-container" onScrollEnd={onScrolled}>
                            {data.map(card => (
                                <div className="flash-card-list__flash-card" key={card.id}>
                                    <FlashCardListItem flashCard={card}></FlashCardListItem>
                                </div>
                            ))}
                        </div>
                    )
            }
            
        </div>
    );
}

export default FlashCardList;
