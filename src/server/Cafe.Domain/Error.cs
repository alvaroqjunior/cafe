﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Cafe.Domain
{
    public readonly struct Error
    {
        public Error(IEnumerable<string> messages)
            : this(messages.ToArray())
        {
        }

        public Error(params string[] messages)
        {
            Messages = messages;
            Date = DateTime.Now;
        }

        public IReadOnlyList<string> Messages { get; }

        public DateTime Date { get; }

        public static Error FromString(string error) =>
            new Error(error);

        public static Error FromCollection(IEnumerable<string> errors) =>
            new Error(errors);

        public static implicit operator Error(string message) =>
            new Error(message);

        public static implicit operator Error(string[] messages) =>
            new Error(messages);

        public static implicit operator Error(Error[] errors) =>
            errors.Aggregate(MergeErrors);

        public static Error MergeErrors(Error first, Error second) =>
            new Error(first.Messages.Concat(second.Messages));
    }
}