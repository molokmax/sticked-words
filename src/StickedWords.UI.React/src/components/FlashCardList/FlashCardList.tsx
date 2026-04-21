import { UIEvent, useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { Link } from 'react-router';
import { FlashCardShort } from '../../models/FlashCardShort';
import { PageQuery } from '../../models/PageQuery';
import { ErrorHandler } from '../../services/ErrorHandler';
import { FlashCardService } from '../../services/FlashCardService';
import { useErrorListContext } from '../ErrorList';
import FlashCardListItem from '../FlashCardListItem';
import Loading from '../Loading';

import './FlashCardList.scss';

function FlashCardList() {

    const [loading, setLoading] = useState(false);
    const scrollRef = useRef<HTMLDivElement>(null);
    const [total, setTotal] = useState(0);
    const [data, setData] = useState<FlashCardShort[]>([]);
    const { addError } = useErrorListContext();

    const service = useMemo(() => new FlashCardService(), []);
    const dataLength = useMemo(() => data.length, [data.length]);

    const loadData = useCallback((reload = false) => {
        const scrollTop = scrollRef.current?.scrollTop ?? 0;
        const skip = reload ? 0 : dataLength;
        const query = new PageQuery(reload, skip);
        setLoading(true);
        service.getByQuery(query)
            .then(page => {
                reload ? setData(page.data) : setData(oldData => [...oldData, ...page.data]);
                if (page.total != null) { // null or undefined
                    setTotal(page.total);
                }
            })
            .catch(err => {
                if (reload) {
                    setData([]);
                    setTotal(0);
                }
                addError(ErrorHandler.getMessage(err));
            })
            .finally(() => {
                setLoading(false);
                if (scrollRef.current) {
                    scrollRef.current.scrollTop = scrollTop;
                }
            });
    }, [service, addError, dataLength]);

    function onScrolled(ev: UIEvent<HTMLDivElement>) {
        const container = ev.currentTarget;
        const threshold = 100;
        const toBottom = container.scrollHeight - (container.scrollTop + container.clientHeight);
        if (toBottom < threshold && !loading && data.length < total) {
            loadData();
        }
    };

    useEffect(() => loadData(true), [loadData]);

    return (
        <div className="flash-card-list">
            <div className="flash-card-list__header">
                <Link to="add" className="flash-card-list__add-word">Add</Link>
                <Link to="session" className="flash-card-list__start-session">Learn</Link>
                <div className="flash-card-list__words-count">Words: { total }</div>
            </div>
            {
                <div
                    className="scroll-container"
                    ref={ scrollRef }
                    onScrollEnd={onScrolled}
                >
                    {data.map(card => (
                        <div className="flash-card-list__flash-card" key={card.id}>
                            <FlashCardListItem flashCard={card}></FlashCardListItem>
                        </div>
                    ))}
                    {
                        loading ? <Loading /> : null 
                    }
                </div>
            }
            
        </div>
    );
}

export default FlashCardList;
