import React from "react";
import { TicketEvent } from "./event";

export type EventCardProps = {
    event: TicketEvent
}

export const EventCard: React.FC<EventCardProps> = ({ event }) => {
    return (
        <div className="event-card">
            <div className="event-card__img-wrapper">
                <img src={event.photo} />
                <div className="event-card__label">
                    <p>от {event.price} ₽</p>
                </div>
            </div>
            <p className="event-card__artist">{event.artist}</p>
            <p className="event-card__eventInfo">{event.date} • {event.place}</p>
        </div>
    )
}