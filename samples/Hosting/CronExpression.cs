using System;
using System.Collections;

namespace Hosting
{
    public class CronExpression
    {
        public string Minute { get; set; } = "*";
        public string Hour { get; set; } = "*";
        public string Day { get; set; } = "*";
        public string Month { get; set; } = "*";
        public string DayOfWeek { get; set; } = "*";

        public static CronExpression Parse(string exp)
        {
            var expressionList = exp.Split(' ');
            if (expressionList.Length != 5)
            {
                throw new ArgumentException("Expression is not formatted correctly.");
            }

            return new CronExpression
            {
                Minute = expressionList[0].Trim(),
                Hour = expressionList[1].Trim(),
                Day = expressionList[2].Trim(),
                Month = expressionList[3].Trim(),
                DayOfWeek = expressionList[4].Trim(),
            };
        }

        public override string ToString()
        {
            return $"{Minute} {Hour} {Day} {Month} {DayOfWeek}";
        }

        protected enum CronExpressionParameter
        {
            Minute,
            Hour,
            Day,
            Month,
            DayOfWeek
        }

        protected ArgumentOutOfRangeException LimitException(CronExpressionParameter paramType)
        {
            switch (paramType)
            {
                case CronExpressionParameter.Minute:
                    return new ArgumentOutOfRangeException("Minute must be between 0 and 60 (exclusive).");
                case CronExpressionParameter.Hour:
                    return new ArgumentOutOfRangeException("Minute must be between 0 and 24 (exclusive).");
                case CronExpressionParameter.Day:
                    return new ArgumentOutOfRangeException("Minute must be between 0 and 31 (exclusive).");
                case CronExpressionParameter.Month:
                    return new ArgumentOutOfRangeException("Minute must be between 0 and 12 (exclusive).");
                case CronExpressionParameter.DayOfWeek:
                    return new ArgumentOutOfRangeException("Minute must be between 0 and 7 (exclusive).");
            }

            return new ArgumentOutOfRangeException();
        }

        protected bool IsWithinLimit(int param, CronExpressionParameter paramType)
        {
            if (param < 0)
            {
                return false;
            }

            switch (paramType)
            {
                case CronExpressionParameter.Minute:
                    return param < 60;
                case CronExpressionParameter.Hour:
                    return param < 24;
                case CronExpressionParameter.Day:
                    return param < 31;
                case CronExpressionParameter.Month:
                    return param < 12;
                case CronExpressionParameter.DayOfWeek:
                    return param < 7;
            }

            return false;
        }

        protected void SetParam(string value, CronExpressionParameter paramType)
        {
            switch (paramType)
            {
                case CronExpressionParameter.Minute:
                    Minute = value;
                    break;
                case CronExpressionParameter.Hour:
                    Hour = value;
                    break;
                case CronExpressionParameter.Day:
                    Day = value;
                    break;
                case CronExpressionParameter.Month:
                    Month = value;
                    break;
                case CronExpressionParameter.DayOfWeek:
                    DayOfWeek = value;
                    break;
            }
        }

        protected CronExpression Set(int value, CronExpressionParameter paramType)
        {
            if (!IsWithinLimit(value, paramType))
            {
                throw LimitException(paramType);
            }

            SetParam($"{value}", paramType);

            return this;
        }

        protected CronExpression SetEvery(CronExpressionParameter paramType)
        {
            SetParam("*", paramType);

            return this;
        }

        protected CronExpression SetList(IEnumerable values, CronExpressionParameter paramType)
        {
            foreach (var value in values)
            {
                if (!IsWithinLimit((int)value, paramType))
                {
                    throw LimitException(paramType);
                }
            }

            //SetParam($"{string.Join(",", values)}", paramType);

            return this;
        }

        protected CronExpression SetRange(int lowerValue, int higherValue, CronExpressionParameter paramType)
        {
            if (!IsWithinLimit(lowerValue, paramType) && !IsWithinLimit(higherValue, paramType))
            {
                throw LimitException(paramType);
            }

            SetParam($"{lowerValue}-{higherValue}", paramType);

            return this;
        }

        public CronExpression SetMinute(int minute) => Set(minute, CronExpressionParameter.Minute);
        public CronExpression SetMinute(IEnumerable minute) => SetList(minute, CronExpressionParameter.Minute);
        public CronExpression SetMinute(int start, int end) => SetRange(start, end, CronExpressionParameter.Minute);
        public CronExpression SetEveryMinute() => SetEvery(CronExpressionParameter.Minute);

        public CronExpression SetHour(int hour) => Set(hour, CronExpressionParameter.Hour);
        public CronExpression SetHour(IEnumerable hour) => SetList(hour, CronExpressionParameter.Hour);
        public CronExpression SetHour(int start, int end) => SetRange(start, end, CronExpressionParameter.Hour);
        public CronExpression SetEveryHour() => SetEvery(CronExpressionParameter.Hour);

        public CronExpression SetDay(int day) => Set(day, CronExpressionParameter.Day);
        public CronExpression SetDay(IEnumerable day) => SetList(day, CronExpressionParameter.Day);
        public CronExpression SetDay(int start, int end) => SetRange(start, end, CronExpressionParameter.Day);
        public CronExpression SetEveryDay() => SetEvery(CronExpressionParameter.Day);

        public CronExpression SetMonth(int day) => Set(day, CronExpressionParameter.Month);
        public CronExpression SetMonth(IEnumerable day) => SetList(day, CronExpressionParameter.Month);
        public CronExpression SetMonth(int start, int end) => SetRange(start, end, CronExpressionParameter.Month);
        public CronExpression SetEveryMonth() => SetEvery(CronExpressionParameter.Month);

        public CronExpression SetDayOfWeek(int day) => Set(day, CronExpressionParameter.DayOfWeek);
        public CronExpression SetDayOfWeek(IEnumerable day) => SetList(day, CronExpressionParameter.DayOfWeek);
        public CronExpression SetDayOfWeek(int start, int end) => SetRange(start, end, CronExpressionParameter.DayOfWeek);
        public CronExpression SetEveryDayOfWeek() => SetEvery(CronExpressionParameter.Day);
    }
}
