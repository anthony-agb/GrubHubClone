﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public readonly record struct OrderStatusUpdatedMessage(
    Guid Id,
    string Status
    );